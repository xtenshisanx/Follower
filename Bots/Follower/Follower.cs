using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using Loki.Bot;
using Loki.Bot.Pathfinding;
using Loki.Bot.Logic;
using Loki.Bot.Logic.Behaviors;
using Loki.Bot.Prototypes;
using Loki.Game;
using Loki.Game.Dat;
using Loki.Game.Inventory;
using Loki.Game.Objects;
using Loki.TreeSharp;
using Loki.Utilities;

using log4net;
using Loki.Game.Objects.Components;
using System.IO;
using System.Collections.Generic;

using Action = Loki.TreeSharp.Action;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Loki.Game.NativeWrappers;

namespace Follower
{
    public class Follower : IBot
    { 

        public Boolean RequiresGameInput { get { return true; } }

        public PulseFlags PulseFlags { get { return PulseFlags.All; } }

        public static ILog Log = Logger.GetLoggerInstanceForType();

        public System.Windows.Window ConfigWindow
        {
            get
            {
                new SettingsWindow().ShowDialog();
                return null;
            }
        }

        private Int32 DefaultTPS = 0;
        private Int32 WantedTPS = 20;

        public static Leader leader = new Leader(Settings.Instance.leaderName);
        public static WaitTimer recheckForNewItems = new WaitTimer(TimeSpan.FromSeconds(5));

        public String Name { get { return "Follower by xTenshiSanx"; } }
        public String Description { get { return "Follows a definied Character and fight with him"; } }

        public override String ToString() { return Name; }
        
        private static Composite botLogic;
        public Composite Logic { get { return botLogic ?? (botLogic = SharedCode.MainLogic(() => SharedCode.RootLogic(TownLogic, () => SharedCode.OutOfTownLogic(DeadLogic, AliveLogic)))); } }

        #region Dispose
        /// <summary>
        /// This function will be called when the Bot frees some ram
        /// </summary>
        public void Dispose()
        {
        
        }
        #endregion
        #region Start
        /// <summary>
        /// This Function is called when the Bot starts
        /// </summary>
        public void Start()
        {
            DefaultTPS = BotMain.TicksPerSecond;
            BotMain.TicksPerSecond = WantedTPS;
        }
        #endregion
        #region Stop
        /// <summary>
        /// This Function is called when the Bot stops
        /// </summary>
        public void Stop()
        {
            BotMain.TicksPerSecond = DefaultTPS;
            Variables.ResetVariables();
        }
        #endregion
        #region Pulse
        /// <summary>
        /// This Function is called every Tick
        /// </summary>
        public void Pulse()
        {
            //Stops the Bot if Leader not in Party
            if(leader.PartyMember == null)
            {
                BotMain.Stop("Leader is not in Party");
            }
            //Reset townvariables if im not in town
            if (LokiPoe.Me.IsInTown && LokiPoe.IsInGame && Variables.areTownVarsRested)
                Variables.areTownVarsRested = false;
            else if (!LokiPoe.Me.IsInTown && LokiPoe.IsInGame && !Variables.areTownVarsRested)
            {
                Variables.areTownVarsRested = true;
                Variables.ResetVariables();
                Variables.isWaitingForMaster = false;
            }
            //Chicken
            if (LokiPoe.Me.HealthPercent <= Settings.Instance.chickenHealthPoints)
                GuiApi.Logout();
        }
        #endregion

        #region TownLogic
        /// <summary>
        /// Townlogic will be run by Bot if he's in town
        /// </summary>
        private static Composite TownLogic()
        {
            return new PrioritySelector(
                new Decorator(ret => !Variables.isTownRunStarted, BotLogic.DoStartTownRun()),
                new Decorator(ret => Variables.isTownRunStarted && !Variables.vendored, BotLogic.DoVendor()),
                new Decorator(ret => Variables.isTownRunStarted && Variables.vendored && !Variables.stashed, BotLogic.DoStashing()),
                new Decorator(ret => !Variables.isTownRunFinished, BotLogic.DoFinishTownRun()),
                new PrioritySelector(
                    new Decorator(ret => Variables.cameByTownPortal, BotLogic.TakeTownPortal(true)),
                    new Decorator(ret => Variables.cameByWaypoint && !leader.IsInTown && GuiApi.Waypoint.AvailableWaypoints.Contains(leader.Area), BotLogic.TakeWaypoint()),
                    new Decorator(ret => !Variables.cameByWaypoint && !Variables.cameByTownPortal && !leader.IsInTown && GuiApi.Waypoint.AvailableWaypoints.Contains(leader.Area), BotLogic.TakeWaypoint())),
                    new Decorator(ret => !Variables.cameByWaypoint && !Variables.cameByTownPortal && !leader.IsInTown && !GuiApi.Waypoint.AvailableWaypoints.Contains(leader.Area), BotLogic.TakeTownPortal(true))

            );
        }
        #endregion
        #region DeadLogic
        /// <summary>
        /// Deadlogic will be run by Bot if he dies
        /// </summary>
        private static Composite DeadLogic()
        {
            return new PrioritySelector(
                new Action(ret =>
                {

                    return RunStatus.Success;
                })
            );
        }
        #endregion
        #region AliveLogic
        /// <summary>
        /// Alivelogic will be run by Bot if he's out of town
        /// </summary>
        private static Composite AliveLogic()
        {
            return new PrioritySelector(
                BotLogic.Fight(),
                new Decorator(ret => LokiPoe.EntityManager.Waypoint() != null && !Functions.DoIHaveWaypointOfThisArea(), BotLogic.GetWaypoint()),
                new Decorator(ret => leader.Player == null && LokiPoe.EntityManager.OfType<Portal>().Count(d => d.Distance <= 40) > 0, BotLogic.TakeTownPortal(false)),
                new Decorator(ret => leader.Player == null && LokiPoe.EntityManager.Waypoint().Distance <= 40, BotLogic.TakeWaypoint()),
                new Decorator(ret => leader.Player == null && LokiPoe.EntityManager.OfType<AreaTransition>().Count(d => d.Distance <= 40) > 0, new Action(delegate {LokiPoe.EntityManager.OfType<AreaTransition>().FirstOrDefault().Interact();})),

                new Decorator(ret => Loki.Bot.Pathfinding.Navigator.MoveCommand != null, BotLogic.FinishMove()),

                new Decorator(ret => !Functions.AreEnemysInRange(50), BotLogic.TakeLoot()),
                new Decorator(ret => !Functions.AreEnemysInRange(50), BotLogic.OpenChests()),

                new Decorator(Follow => leader.Player != null && leader.Player.Distance > 20, CommonBehaviors.MoveTo(ret => SharedLogic.GetWalkablePositionNear(leader.Player), ret => "Follower(): Following Leader"))
            );
        }
        #endregion
    }
}
