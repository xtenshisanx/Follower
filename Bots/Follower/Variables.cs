using System;
using System.Collections;
using System.Collections.Generic;
using Loki.Game;
using Loki.Game.Inventory;
using Loki.Game.NativeWrappers;
using Loki.Utilities;

namespace Follower
{
    class Variables
    {
        public static Boolean areTownVarsRested = false;
        public static Boolean isBotStarted = false;
        public static Boolean isTownRunStarted = false;
        public static Boolean isTownRunFinished = false;
        public static Boolean isWaitingForMaster = false;
        public static Boolean vendored = false;
        public static Boolean stashed = false;
        public static Boolean cameByWaypoint = false;
        public static Boolean cameByTownPortal = false;
        public static Boolean cameByTransistion = false;

        public static readonly WaitTimer moveItemTimer = new WaitTimer(TimeSpan.FromSeconds(1));
        public static readonly WaitTimer chestTimer = new WaitTimer(TimeSpan.FromSeconds(1));
        public static readonly WaitTimer lootTimer = new WaitTimer(TimeSpan.FromSeconds(1));
        public static readonly WaitTimer waypointTimer = new WaitTimer(TimeSpan.FromSeconds(2));

        public static List<InventoryItem> SellList;
        public static List<InventoryItem> stashList;

        public static Vector2 actOneTownMiddle = new Vector2i(249, 219).MapToWorld();
        public static Vector2 actTwoTownMiddle = new Vector2i(184, 168).MapToWorld();
        public static Vector2 actThreeTownMiddle = new Vector2i(231, 300).MapToWorld();

        public static String actOneVendor = "Nessa";
        public static String actTwoVendor = "Greust";
        public static String actThreeVendor = "Hargan";

        public static Int32[] actOne =   {0,  // Lioneye's Watch
                                        4,  // The Mud Flats
                                        7,  // The Submerged Passage 
                                        9,  // The Ledge
                                        10, // The Climb
                                        11, // The Prison
                                        13, // Prisoner's Gate
                                        14, // The Ship Graveyard
                                        17}; // The Cavern of Wrath
        public static Int32[] actTwo =   {19, // The Forrest Encampment
                                        20, // The Southern Forest
                                        23, // The Crossroads
                                        24, // The Crypt Level 1
                                        27, // The Chamber of Sins Level 2
                                        29, // The Broken Bridge
                                        31, // The Blackwood
                                        32, // The Western Forest
                                        34, // The Vaal Ruins Level 2
                                        36, // The Wetlands
                                        39}; // The Caverns Level 2
        public static Int32[] actThree = {42, // The Sarn Encampment
                                        43, // The City of Sarn
                                        45, // The Crematorium
                                        54, // The Warehouse Sewers
                                        47, // The Marketplace
                                        49, // The Battlefront
                                        51, // The Solaris Temple
                                        52, // The Docks
                                        57, // The (Ebony-)Barracks
                                        59, // The Lunaris Temple
                                        61, // The Imperial Gardens
                                        63, // The Library
                                        65}; // The Sceptre God

        #region ResetVariables
        /// <summary>
        /// Resets the Variables to default after townrun or when the Bot stops
        /// </summary>
        public static void ResetVariables()
        {
            SellList = null;
            stashList = null;
            isTownRunStarted = false;
            isTownRunFinished = false;
            stashed = false;
            vendored = false;
            cameByWaypoint = false;
            cameByTownPortal = false;
            cameByTransistion = false;
        }
        #endregion
    }
}
