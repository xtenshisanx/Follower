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
    class Leader
    {
        public String name;
        
        #region PlayerEntry
        /// <summary>
        /// Returns the PlayerEntry from Friendlist of Leader
        /// </summary>
        public PlayerEntry PlayerEntry
        {
            get
            {
                return LokiPoe.CurrentInstance.FriendList.FirstOrDefault<PlayerEntry>(player => player.CharacterName.Equals(this.name));
            }
            
        }
        #endregion
        #region PartyMember
        /// <summary>
        /// Returns the PartyMember object of Leader
        /// </summary>
        public PartyMember PartyMember
        {
            get
            {
                return LokiPoe.CurrentInstance.PartyMembers.FirstOrDefault<PartyMember>(player => player.PlayerEntry.CharacterName.Equals(this.name));
            }
            
        }
        #endregion
        #region Player
        /// <summary>
        /// Returns the Leader's playerobject
        /// </summary>
        public Player Player
        {
            get
            {
                return LokiPoe.EntityManager.OfType<Player>().FirstOrDefault(player => player.Name.Equals(this.name));
            }
            
        }
        #endregion
        #region IsInTown
        /// <summary>
        /// Returns TRUE if the Leader is in town
        /// </summary>
        public Boolean IsInTown
        {
            get
            {
                return Functions.GetAreaStringByNumber(PartyMember.PlayerEntry.AreaId, Settings.difficulty).Contains("town");
            }
            
        }
        #endregion
        #region IsInParty
        /// <summary>
        /// Checks if the Leader is in party
        /// </summary>
        public Boolean IsInParty
        {
            get
            {
                return PartyMember != null;
            }
            
        }
        #endregion
        #region Act
        /// <summary>
        /// Returns the Leader's act
        /// </summary>
        public Int32 Act
        {
            get
            {
                Int32 result = 0;
                String area = Functions.GetAreaStringByNumber(PartyMember.PlayerEntry.AreaId, Settings.difficulty);
                if (area[2].ToString().Equals("1"))
                    result = 1;
                else if (area[2].ToString().Equals("2"))
                    result = 2;
                else if (area[2].ToString().Equals("3"))
                    result = 3;
                return result;
            }
            
        }
        #endregion
        #region Area
        /// <summary>
        /// Returns Leader's area
        /// </summary>
        public WorldAreaEntry Area
        {
            get
            {
                WorldAreaEntry result = null;
                String area = Functions.GetAreaStringByNumber(PartyMember.PlayerEntry.AreaId, Settings.difficulty);
                foreach (WorldAreaEntry _entry in GuiApi.Waypoint.AvailableWaypoints)
                {
                    if (_entry.Id == area)
                    {
                        result = _entry;
                    }
                }
                return result;
            }
        }
        #endregion
        #region Constructor Leader
        /// <summary>
        /// Creates a new Leader with given name
        /// </summary>
        /// <param name="leaderName">Leadername e.x. LetsEndThis</param>
        public Leader(String leaderName)
        {
            this.name = leaderName;
        }
        #endregion
    }
}
