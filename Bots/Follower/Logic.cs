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

using Follower;

namespace Follower
{
    public class BotLogic
    {
        public delegate T Typ<out T>(object context);
        #region DoFinishTownRun
        /// <summary>
        /// Finishes the townrun and moves to the townmiddle
        /// </summary>
        public static Composite DoFinishTownRun()
        {
            return new PrioritySelector(
                new Sequence(
                    new Switch<Int32>(ret => Functions.GetActOfMe(),
                        new SwitchArgument<Int32>(1, new Sequence(
                            new DecoratorContinue(ret => Variables.actOneTownMiddle.Distance(LokiPoe.Me.Position.MapToWorld()) > 30, CommonBehaviors.MoveTo(ret => Variables.actOneTownMiddle.WorldToMap(), ret => "Follower(DoFinishTownRun): Move to Townmiddle A1")),
                            new WaitContinue(10, ret => Variables.actOneTownMiddle.Distance(LokiPoe.Me.Position.MapToWorld()) <= 30, new Action(delegate { return RunStatus.Success; })))),
                        new SwitchArgument<Int32>(2, new Sequence(
                            new DecoratorContinue(ret => Variables.actTwoTownMiddle.Distance(LokiPoe.Me.Position.MapToWorld()) > 30, CommonBehaviors.MoveTo(ret => Variables.actTwoTownMiddle.WorldToMap(), ret => "Follower(DoFinishTownRun): Move to Townmiddle A2")),
                            new WaitContinue(10, ret => Variables.actTwoTownMiddle.Distance(LokiPoe.Me.Position.MapToWorld()) <= 30, new Action(delegate { return RunStatus.Success; })))),
                        new SwitchArgument<Int32>(3, new Sequence(
                            new DecoratorContinue(ret => Variables.actThreeTownMiddle.Distance(LokiPoe.Me.Position.MapToWorld()) > 30, CommonBehaviors.MoveTo(ret => Variables.actThreeTownMiddle.WorldToMap(), ret => "Follower(DoFinishTownRun): Move to Townmiddle A3")),
                            new WaitContinue(10, ret => Variables.actThreeTownMiddle.Distance(LokiPoe.Me.Position.MapToWorld()) <= 30, new Action(delegate { return RunStatus.Success; }))))
                    ),
                    new Action(ret => { Follower.Log.Debug("Follower(DoFinishTownRun): Townrun Finished!"); }),
                    new Action(delegate { Variables.isTownRunFinished = true; return RunStatus.Success; })
                ));
        }
        #endregion
        #region DoStartTownRun
        /// <summary>
        /// Starts the townrun and generates the selllist after moving to townmiddle
        /// </summary>
        public static Composite DoStartTownRun()
        {
            return new PrioritySelector(
                new Sequence(
                    new Action(_start_townrun => { Follower.Log.Debug("Follower(DoStartTownRun): Townrun Started!"); }),
                //Move to Townmiddle
                    new Switch<Int32>(ret => Functions.GetActOfMe(),
                        new SwitchArgument<Int32>(1, new Sequence(
                            new DecoratorContinue(ret => Variables.actOneTownMiddle.Distance(LokiPoe.Me.Position.MapToWorld()) > 30, CommonBehaviors.MoveTo(ret => Variables.actOneTownMiddle.WorldToMap(), ret => "Follower(DoFinishTownRun): Move to Townmiddle A1")),
                            new WaitContinue(10, ret => Variables.actOneTownMiddle.Distance(LokiPoe.Me.Position.MapToWorld()) <= 30, new Action(delegate { return RunStatus.Success; })))),
                        new SwitchArgument<Int32>(2, new Sequence(
                            new DecoratorContinue(ret => Variables.actTwoTownMiddle.Distance(LokiPoe.Me.Position.MapToWorld()) > 30, CommonBehaviors.MoveTo(ret => Variables.actTwoTownMiddle.WorldToMap(), ret => "Follower(DoFinishTownRun): Move to Townmiddle A2")),
                            new WaitContinue(10, ret => Variables.actTwoTownMiddle.Distance(LokiPoe.Me.Position.MapToWorld()) <= 30, new Action(delegate { return RunStatus.Success; })))),
                        new SwitchArgument<Int32>(3, new Sequence(
                            new DecoratorContinue(ret => Variables.actThreeTownMiddle.Distance(LokiPoe.Me.Position.MapToWorld()) > 30, CommonBehaviors.MoveTo(ret => Variables.actThreeTownMiddle.WorldToMap(), ret => "Follower(DoFinishTownRun): Move to Townmiddle A3")),
                            new WaitContinue(10, ret => Variables.actThreeTownMiddle.Distance(LokiPoe.Me.Position.MapToWorld()) <= 30, new Action(delegate { return RunStatus.Success; }))))
                    ),
                //Generate Sellarray
                    new Action(delegate
                    {
                        Inventory myInventory = LokiPoe.Me.Inventory.GetInventoryById(0);
                        if (Variables.SellList == null || Variables.SellList.Count == 0)
                        {
                            Variables.SellList = new List<InventoryItem>();
                            foreach (InventoryItem _item in myInventory.Items)
                            {

                                if (Registry.ItemEvaluator.Match(_item.Item, EvaluationType.Stash) == null && _item.Item.Rarity < Rarity.Rare)
                                {
                                    Variables.SellList.Add(_item);
                                    Follower.Log.DebugFormat("Follower(DoStartTownRun): Added {0} to Selllist", _item.Name);
                                }
                            }
                            Follower.Log.DebugFormat("Follower(DoStartTownRun): Items to Sell: {0}", Variables.SellList.Count.ToString());
                            Variables.isTownRunStarted = true;
                        }
                    })
                ),
                new Action(_finish_Start => { return RunStatus.Success; })
            );
        }
        #endregion
        #region DoVendor
        /// <summary>
        /// Do vendoring, Sells crap stuff
        /// </summary>
        public static Composite DoVendor()
        {
            PoEObject vendor = null;
            Int32 TalkTry = 0;
            return new PrioritySelector(
                //Check if we need to vendor
                    new Decorator(param0 => Variables.SellList.Count <= 0, new Action(delegate
                        {
                            Follower.Log.Debug("Follower(DoVendor): We not need to Vendor. Skipping!");
                            Variables.vendored = true;
                            SharedCode.SetForcedWait(1000f);
                            return RunStatus.Success;
                        }
                    )),
                new Sequence(
                //Get Vendor
                    new Decorator(_get_vendor => vendor == null, new Action(delegate
                    {
                        if (Functions.GetActOfMe() == 1)
                            vendor = SharedLogic.GetNpcByName(Variables.actOneVendor);
                        if (Functions.GetActOfMe() == 2)
                            vendor = SharedLogic.GetNpcByName(Variables.actTwoVendor);
                        if (Functions.GetActOfMe() == 3)
                            vendor = SharedLogic.GetNpcByName(Variables.actThreeVendor);
                    })),
                    new Decorator(_get_vendor => vendor != null, new Action(delegate { Follower.Log.Debug("Follower(DoVendor): Found Vendor! Distance: " + vendor.Distance.ToString()); })),
                //Move to Vendor
                    new DecoratorContinue(_move_to_vendor => vendor.Distance > 20, CommonBehaviors.MoveTo(ret => vendor.Position, ret => "Follower(DoVendor): Vendor is out of range")),
                //Wait for Range
                    new Wait(5, ret => vendor.Distance <= 20, new Action(delegate { return RunStatus.Success; })),
                //Interact with Vendor
                    new PrioritySelector(new Action(delegate {
                        if (GuiApi.Npc.DialogOptions.Count() > 0)
                        {
                            return RunStatus.Success;
                        }
                        Follower.Log.DebugFormat("Follower(DoVendor): Interacting with {0} ID: {1}",vendor.Name,vendor.ID.ToString());
                        vendor.Interact();
                        TalkTry++;
                        SharedCode.SetForcedWait(3000f, 0f);
                        return RunStatus.Running;
                    })),
                //Wait for DialogWindow
                    new Wait(5, ret => GuiApi.Npc.DialogOptions.Count() > 0, new Action(delegate { return RunStatus.Success; })),
                //Open TradeWindow
                    new Decorator(_open_trade_window => GuiApi.Npc.DialogOptions.Count() != 0, new Action(do_open_trade_window =>
                    {
                        Int32 Sellbutton = GuiApi.Npc.DialogOptions.FirstOrDefault(buttons => buttons.DialogText.Contains("Sell")).DialogId;
                        GuiApi.Npc.SelectDialogOption(Sellbutton);
                    })),
                //Wait for TradeWindow
                    new Wait(TimeSpan.FromSeconds(2), ret => GuiApi.Npc.IsNpcTradeWindowOpen, new Action(action => { return RunStatus.Success; })),
                //Sell Items
                    new PrioritySelector(
                        new Action(delegate
                        {
                            foreach(InventoryItem item in Variables.SellList)
                            {
                                item.FastMoveToTradeWindow();
                                SharedCode.SetForcedWait(1000f, 0f);
                            }
                        })),
                //Accept Trade
                    new WaitContinue(1, ret => false, new Action(delegate { return RunStatus.Success; })),
                    new Decorator(ret => GuiApi.Npc.IsNpcTradeWindowOpen , new Action(delegate { GuiApi.Npc.AcceptCurrentNpcTrade(); })),
                //Wait for DialogWindow
                    new WaitContinue(1, ret => false, new Action(delegate { return RunStatus.Success; })),
                //Close DialogWindow
                    new Decorator(ret => GuiApi.Npc.DialogOptions.Count() > 0 , new Action(delegate
                    {
                        GuiApi.Npc.CloseNpcDialogWindow();
                        Variables.vendored = true;
                        vendor = null;
                        TalkTry = 0;
                        return RunStatus.Success;
                    })),
                    new WaitContinue(1, ret => false, new Action(delegate { return RunStatus.Success; }))
                )
            );
        }
        #endregion
        #region DoStashing
        /// <summary>
        /// Do stashing, stashes all currencys or other items set by filters
        /// </summary>
        public static Composite DoStashing()
        {
            PoEObject Stash = null;
            Int32 StashTry = 0;
            return new PrioritySelector(
                new Sequence(
                //Find Stash
                    new Decorator(_get_stash => Stash == null, new Action(ret => { Follower.Log.Debug("Follower(DoStashing): Start Stashinglogic."); Stash = LokiPoe.EntityManager.Stash(); })),
                    new Decorator(_found_stash => Stash != null, new Action(ret => { Follower.Log.Debug("Follower(DoStashing): Found Stash."); })),
                //Move To Stash
                    new DecoratorContinue(_move_to_stash => Stash.Distance > 30, CommonBehaviors.MoveTo(ret => Stash.Position, ret => "MoveToSTash")),
                //Wait for Range
                    new WaitContinue(5, ret => Stash.Distance > 10, new Action(delegate { return RunStatus.Success; })),
                //Interact with Chest
                    new PrioritySelector(
                        new Action(delegate
                        {
                            if (Variables.moveItemTimer.IsFinished)
                            {
                                Follower.Log.DebugFormat("Follower(DoStashing): Interacting With {0} (try : {1})", Stash.Name, StashTry);
                                StashTry++;
                                Stash.Interact();
                                Variables.moveItemTimer.Reset();
                                if (GuiApi.IsStashWindowOpen)
                                    return RunStatus.Success;
                                return RunStatus.Running;
                            }
                            return RunStatus.Running;
                        })),
                //Wait for Stashwindow
                    new Wait(10, ret => GuiApi.IsStashWindowOpen, new Action(delegate { return RunStatus.Success; })),
                //Generate Stashing Array
                    new Decorator(_generate_stasharray => GuiApi.IsStashWindowOpen, new Action(do_generate_stash_array =>
                    {
                        Inventory MyInventory = LokiPoe.Me.Inventory.GetInventoryById(0);
                        if (Variables.stashList == null)
                        {
                            Variables.stashList = new List<InventoryItem>();
                            foreach (InventoryItem _item in MyInventory.Items)
                            {
                                Variables.stashList.Add(_item);
                                Follower.Log.DebugFormat("Follower(DoStartTownRun): Added {0} to Selllist", _item.Name);
                            }
                        }
                        if (Variables.stashList.Count != 0)
                        {
                            Follower.Log.DebugFormat("Follower(DoStashing): Items to stash:", Variables.stashList.Count);
                        }
                    })),
                //Stashing Items
                    new WaitContinue(1, ret => false, new Action(delegate { return RunStatus.Success; })),
                    new PrioritySelector(
                        new Action(delegate
                        {
                            InventoryItem _Item = null;
                            StashInventory MyStash = null;
                            Int32 items = Variables.stashList.Count;
                            if (Variables.moveItemTimer.IsFinished && Variables.stashList != null)
                            {
                                MyStash = LokiPoe.Me.Inventory.GetStashTab(0);
                                _Item = Variables.stashList.FirstOrDefault();
                            }
                            if (_Item != null)
                            {
                                Follower.Log.Debug("Follower(DoStashing): " + items.ToString() + " Items left");
                                _Item.FastMove(MyStash);
                                Variables.stashList.Remove(_Item);
                                items--;
                                Variables.moveItemTimer.Reset();
                            }
                            if (items != 0)
                            {
                                return RunStatus.Running;
                            }
                            return RunStatus.Success;
                        })),
                //Close Stash
                    new WaitContinue(1, ret => false, new Action(delegate { return RunStatus.Success; })),
                    new Action(delegate
                    {
                        new Input().SendHotkeyInput(HotkeyAction.ClosePanels);
                        Variables.stashed = true;
                        Follower.Log.Debug("Follower(DoStashing): Finished Stashing!");
                        Stash = null;
                        StashTry = 0;
                        return RunStatus.Success;
                    })
                )
            );
        }
        #endregion
        #region WaitForMaster
        /// <summary>
        /// Moves to the position of Waypoint, TownPortal or Transistion and waits for Master
        /// </summary>
        public static Composite WaitForMaster()
        {
            return new PrioritySelector(
                new Decorator(_tp => Variables.cameByTownPortal, new Sequence(
                    new DecoratorContinue(ret => SharedLogic.GuessPortalLocation().Distance(LokiPoe.Me.Position) > 30, CommonBehaviors.MoveTo(ret => SharedLogic.GuessPortalLocation(), ret => "Follower(WaitForMaster): Moving to Portalzone")),
                    new WaitContinue(5, ret => SharedLogic.GuessPortalLocation().Distance(LokiPoe.Me.Position) <= 30, new Action(delegate { return RunStatus.Success; })),
                    new Action(delegate
                    {
                        Follower.Log.DebugFormat("Follower(WaitForMaster): Waiting now on Portalzone");
                        Variables.isWaitingForMaster = true;
                    })
                )),
                new Decorator(_wp => Variables.cameByWaypoint, new Sequence(
                    new DecoratorContinue(ret => SharedLogic.GuessWaypointLocation().Distance(LokiPoe.Me.Position) > 30, CommonBehaviors.MoveTo(ret => SharedLogic.GuessWaypointLocation(), ret => "Follower(WaitForMaster): Moving to WaypointLocation")),
                    new WaitContinue(5, ret => SharedLogic.GuessWaypointLocation().Distance(LokiPoe.Me.Position) <= 30, new Action(delegate { return RunStatus.Success; })),
                    new Action(delegate
                    {
                        Follower.Log.DebugFormat("Follower(WaitForMaster): Waiting now on Waypoint");
                        Variables.isWaitingForMaster = true;
                    })
                )),
                new Decorator(_wp => Variables.cameByTransistion, new Sequence(
                )),
                new Decorator(_wp => !Variables.cameByWaypoint && !Variables.cameByTransistion && !Variables.cameByTownPortal, new Sequence(
                    new DecoratorContinue(ret => SharedLogic.GuessWaypointLocation().Distance(LokiPoe.Me.Position) > 30, CommonBehaviors.MoveTo(ret => SharedLogic.GuessWaypointLocation(), ret => "Follower(WaitForMaster): Moving to WaypointLocation")),
                    new WaitContinue(5, ret => SharedLogic.GuessWaypointLocation().Distance(LokiPoe.Me.Position) <= 30, new Action(delegate { return RunStatus.Success; })),
                    new Action(delegate
                    {
                        Follower.Log.DebugFormat("Follower(WaitForMaster): Waiting now on Waypoint");
                        Variables.isWaitingForMaster = true;
                    })
                ))
            );
        }
        #endregion
        #region OpenChests
        /// <summary>
        /// Open Chests in lootDistance
        /// </summary>
        public static Composite OpenChests()
        {
            PoEObject chest = null;
            return new PrioritySelector(
                //Start Sequence
                new Sequence(
                //Check for Chests
                    new Decorator(_check_chests => Variables.chestTimer.IsFinished && Targeting.Loot.Targets.Where(obj => obj is Chest).OrderBy(t => t.Distance).Count(d => d.Distance <= Settings.Instance.lootDistance) > 0,
                //Get a Chest    
                    new Action(_get_chest => { chest = Targeting.Loot.Targets.OfType<Chest>().OrderBy(t => t.Distance).FirstOrDefault(d => d.Distance <= Settings.Instance.lootDistance); })),
                    new DecoratorContinue(_noChest => chest == null, new Action(delegate { return RunStatus.Success; })),
                //Move Into Range if needed
                    new DecoratorContinue(_move_to => chest != null && chest.Distance > 30, CommonBehaviors.MoveTo(ret => chest.Position, ret => "Allrounder(OpenChests): Moving to Chest")),
                //Wait for in Range
                    new WaitContinue(3, _wait_for_distance => chest != null && chest.Distance <= 30, new Action(delegate { return RunStatus.Success; })),
                //Open Chest
                    new Action(delegate { if (chest == null) return RunStatus.Failure; chest.Interact(); Follower.Log.DebugFormat("Follower(OpenChests): Opend up {0}", chest.Name); Variables.chestTimer.Reset(); return RunStatus.Success; }),
                    new WaitContinue(1, ret => false, new Action(delegate { return RunStatus.Success; })),
                    new Action(delegate { return RunStatus.Failure; })));
        }
        #endregion
        #region TakeLoot
        /// <summary>
        /// Gets the loot in lootDistance
        /// </summary>
        public static Composite TakeLoot()
        {
            PoEObject item = null;
            return new PrioritySelector(
                new Sequence(
                    new Decorator(_check_loot => Variables.lootTimer.IsFinished && Targeting.Loot.Targets.Where(obj => obj is WorldItem).OrderBy(t => t.Distance).Count(d => d.Distance <= Settings.Instance.lootDistance) > 0,
                    new Action(_get_item => { item = Targeting.Loot.Targets.OfType<WorldItem>().OrderBy(t => t.Distance).FirstOrDefault(obj => LokiPoe.Me.Inventory.Main.CanFitItem((obj as WorldItem).Item)); })),
                    new DecoratorContinue(_move_to => item.Distance > 30, CommonBehaviors.MoveTo(ret => item.Position, ret => "Allrounder(OpenChests): Moving to Item")),
                    new WaitContinue(3, _wait_for_distance => item != null && item.Distance <= 30, new Action(delegate { return RunStatus.Success; })),
                    new Action(delegate { if (item == null) return RunStatus.Failure; (item as WorldItem).Interact(); Follower.Log.DebugFormat("Follower(TakeLoot): Picked up {0}", (item as WorldItem).Name); Variables.lootTimer.Reset(); return RunStatus.Failure; })));
        }
        #endregion
        #region Fight
        /// <summary>
        /// Initiates the fightcomposite if enemys are in range
        /// </summary>
        public static Composite Fight()
        {
            return new PrioritySelector(
                new Decorator(ret => LokiPoe.EntityManager.OfType<Monster>().Count(a => a.IsActive && !a.IsDead && a.Distance <= 50) > 0,
                RoutineManager.Current.Combat),
                new Action(delegate { return RunStatus.Failure; }));
        }
        #endregion
        #region UseTownPortal
        /// <summary>
        /// Uses the Townportal
        /// </summary>
        private static Composite UseTownPortal()
        {
            WorldArea CurrentWorld = LokiPoe.Me.WorldArea;
            Int32 PortalTry = 0;
            PoEObject Portal = null;
            return new PrioritySelector(
                new Sequence(
                    new Decorator(ret => Portal == null, new Action(retn => { Follower.Log.Debug("Follower(UseTP): Searching Portal of Leader"); Portal = LokiPoe.EntityManager.OfType<Portal>().FirstOrDefault(a => a.IsTargetable); })),
                    new Decorator(ret => Portal != null, new Action(retn => { Follower.Log.Debug("Follower(UseTP): Found Portal"); })),
                    new DecoratorContinue(ret => Portal.Distance > 30, CommonBehaviors.MoveTo(ret => Portal.Position, ret => "Follower(UseTP): Move to Portal")),
                    new WaitContinue(5, ret => Portal.Distance <= 30, new Action(delegate { return RunStatus.Failure; })),
                    new Action(delegate
                    {
                        if (!LokiPoe.Me.IsInTown && !Variables.cameByTownPortal)
                            Variables.cameByTownPortal = true;
                        Logger.GetLoggerInstanceForType().DebugFormat("Allrounder(UseTP): Interacting with Portal {0} on X {1} Y {2} (Try: {3})", Portal.ID, Portal.Position.X, Portal.Position.Y, PortalTry);
                        Portal.Interact();
                        PortalTry++;
                        SharedCode.SetForcedWait(1500f, 0.0f);
                        Portal = null;
                        return;
                    })
                )
            );
        }
        #endregion
        #region CreateTownPortal
        public static Composite CreateTownPortal()
        {
            return new Action(delegate
            {
                InventoryItem portalScroll = LokiPoe.Me.Inventory.Main.FindItem("Portal Scroll");
                if (portalScroll == null)
                {
                    return RunStatus.Failure;
                }
                else
                {
                    portalScroll.Use();
                    SharedCode.SetForcedWait(1000f);
                    return RunStatus.Success;
                }
            }
            );
        }
        #endregion
        #region TakeTownPortal
        /// <summary>
        /// Moves to Townportal and takes it
        /// </summary>
        public static Composite TakeTownPortal(bool inTown)
        {
            if(inTown)
            { 
                return new PrioritySelector(
                    new Decorator(param0 => LokiPoe.Me.Position.Distance(SharedLogic.GuessPortalLocation()) > 15, new Action(delegate
                    {
                        Vector2i PortalLocation = SharedLogic.GuessPortalLocation();
                        Follower.Log.DebugFormat("Follower(TakePortal): Waypoint is to far away. Moving in range");
                        Navigator.BeginMoveTo(new MoveCommand(PortalLocation, "Follower(TakePortal): Waypoint is to far away moving to range", null, null, 3000));
                    })),
                    new Decorator(param0 => LokiPoe.Me.Position.Distance(SharedLogic.GuessPortalLocation()) <= 15 && (!Follower.leader.IsInTown || Follower.leader.Player.Position.Distance(SharedLogic.GuessPortalLocation()) <= 20), UseTownPortal()));
            }
            else
            {
                return new ProbabilitySelector(
                    new Decorator(param0 => LokiPoe.EntityManager.OfType<Portal>().FirstOrDefault() == null, new Sequence(
                        new Action(delegate { Follower.Log.Debug("Follower(TakeTownPortal): Could not found Portal."); }),
                        CreateTownPortal())),
                    new Decorator(param0 => LokiPoe.EntityManager.OfType<Portal>().FirstOrDefault() != null, UseTownPortal()));
            }
        }
        #endregion
        #region UseWaypoint
        /// <summary>
        /// Take waypoint to Leader's area
        /// </summary>
        private static Composite UseWaypoint()
        {
            return new PrioritySelector(
                new Decorator(ret => !GuiApi.Waypoint.IsWorldPanelWindowOpen, new Action(delegate
            {
                PoEObject Waypoint = LokiPoe.EntityManager.Waypoint();
                Navigator.BeginMoveTo((MoveCommand)null);
                Follower.Log.DebugFormat("Follower(TakeWaypoint): Interacting with Waypoint");
                Waypoint.Interact();
                SharedCode.SetForcedWait(GuiApi.Waypoint.IsWorldPanelWindowOpen ? 3000f : 1000f, 0.0f);
            })),
                new Decorator(ret => GuiApi.Waypoint.IsWorldPanelWindowOpen, new Action(delegate
            {
                if (Follower.leader.Area.IsTown && LokiPoe.Me.IsInTown)
                    return;
                Follower.Log.DebugFormat("Follower(TakeWaypoint): Taking Waypoint to Leaders area");
                GuiApi.Waypoint.Take(Follower.leader.Area, false);
                SharedCode.SetForcedWait(100f, 0f);
            })),
                new Action(delegate { return RunStatus.Success; }));
        }
        #endregion
        #region TakeWaypoint
        /// <summary>
        /// Moves to the Waypoint and takes it to Leader's area
        /// </summary>
        public static Composite TakeWaypoint()
        {
            return new PrioritySelector(
                new Decorator(param0 => LokiPoe.EntityManager.Waypoint() == null, new PrioritySelector(
                    new Decorator(param0 => LokiPoe.Me.Position.Distance(SharedLogic.StoredWaypointLocation()) > 15, new Action(delegate
            {
                Vector2i WaypointLocation = SharedLogic.StoredWaypointLocation();
                Follower.Log.DebugFormat("Follower(TakeWaypoint): Waypoint is to far away. Moving in range");
                Navigator.BeginMoveTo(new MoveCommand(WaypointLocation, "Follower(TakeWaypoint): Waypoint is to far away moving to range", null, null, 3000));
            })),
                    new Decorator(param0 => LokiPoe.Me.Position.Distance(SharedLogic.StoredWaypointLocation()) <= 15, UseWaypoint()))
                ),
                new Decorator(param0 => LokiPoe.EntityManager.Waypoint() != null, new PrioritySelector(
                    new Decorator(param0 => LokiPoe.EntityManager.Waypoint().Distance > 15, new Action(delegate
            {
                Vector2i WaypointLocation = LokiPoe.EntityManager.Waypoint().Position;
                Follower.Log.DebugFormat("Follower(TakeWaypoint): Waypoint is to far away. Moving in range");
                Navigator.BeginMoveTo(new MoveCommand(WaypointLocation, "Follower(TakeWaypoint): Waypoint is to far away moving to range", null, null, 3000));
            })),
                    new Decorator(param0 => LokiPoe.Me.Position.Distance(LokiPoe.EntityManager.Waypoint().Position) <= 15, UseWaypoint()))
                )
            );
        }
        #endregion
        #region GetWaypoint
        /// <summary>
        /// Gets the Waypoint if in Range
        /// </summary>
        public static Composite GetWaypoint()
        {
            return new PrioritySelector(
                new Decorator(param0 => LokiPoe.EntityManager.Waypoint() != null,
                    new PrioritySelector(
                        new Decorator(param0 => LokiPoe.EntityManager.Waypoint().Distance > 15, new Action(delegate
                        {
                            Vector2i WaypointLocation = LokiPoe.EntityManager.Waypoint().Position;
                            Follower.Log.DebugFormat("Follower(GetWaypoint): Waypoint is to far away. Moving in range");
                            Navigator.BeginMoveTo(new MoveCommand(WaypointLocation, "Follower(TakeWaypoint): Waypoint is to far away moving to range", null, null, 3000));
                        })),
                        new Decorator(param0 => LokiPoe.Me.Position.Distance(LokiPoe.EntityManager.Waypoint().Position) <= 15,
                            new Decorator(ret => !GuiApi.Waypoint.IsWorldPanelWindowOpen, new Action(delegate
                            {
                                PoEObject Waypoint = LokiPoe.EntityManager.Waypoint();
                                Navigator.BeginMoveTo((MoveCommand)null);
                                Follower.Log.DebugFormat("Follower(GetWaypoint): Interacting with Waypoint");
                                Waypoint.Interact();
                                SharedCode.SetForcedWait(GuiApi.Waypoint.IsWorldPanelWindowOpen ? 3000f : 1000f, 0.0f);
                            }))
                        )
                    )
                )
            );
        }
        #endregion
        #region UseTransistion
        /// <summary>
        /// Uses the next Transistion
        /// </summary>
        public static Composite UseTransistion()
        {
            return new PrioritySelector();
        }
        #endregion
        #region FinishMove
        /// <summary>
        /// Finishes the MoveCommand if needed
        /// </summary>
        public static Composite FinishMove()
        {
            return new Action(param0 => Loki.Bot.Pathfinding.Navigator.MoveCommand.DistanceFromDestination > 25.0 ? RunStatus.Success : RunStatus.Failure);
        }
        #endregion

    }
}
