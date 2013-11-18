using System;
using System.Linq;
using Loki;
using Loki.Game;
using Loki.Game.Objects;
namespace Follower
{
    class Functions
    {
        #region GetActOfMe
        /// <summary>
        /// Returns the current actnumber
        /// </summary>
        /// <returns>Actnumber ex. 1/2/3</returns>
        public static Int32 GetActOfMe()
        {
            Int32 result = 0;
            String area = LokiPoe.CurrentLocalData.WorldAreaId;
            if (area[2].ToString().Equals("1"))
                result = 1;
            else if (area[2].ToString().Equals("2"))
                result = 2;
            else if (area[2].ToString().Equals("3"))
                result = 3;
            return result;
        }
        #endregion
        #region AreEnemysInRange
        /// <summary>
        /// This Function checks for enemys in given range
        /// </summary>
        /// <param name="checkInDistance">distance to looking for</param>
        /// <returns>True/False</returns>
        public static Boolean AreEnemysInRange(Int32 checkInDistance)
        {
            return LokiPoe.EntityManager.OfType<Monster>().Count(obj => obj.Reaction == Reaction.Enemy && !obj.IsDead && obj.Distance <= checkInDistance) > 0;
        }
        #endregion
        #region GetAreaStringByNumber
        /// <summary>
        /// Returns the Areastring of the current Area
        /// </summary>
        /// <param name="areaNumber">Areanumber of Partymember</param>
        /// <param name="difficulty">Difficulty of Partymember</param>
        /// <returns>Areastring of current Area e.x. 1_2_town</returns>
        public static String GetAreaStringByNumber(Int32 areaNumber, Int32 difficulty)
        {
            String result = null;
            if ((areaNumber - 69) == Variables.actOne[0] || (areaNumber - 69 * 2) == Variables.actOne[0] || areaNumber == Variables.actOne[0])
                result = difficulty.ToString() + "_1_town";
            if ((areaNumber - 69) == Variables.actOne[1] || (areaNumber - 69 * 2) == Variables.actOne[1] || areaNumber == Variables.actOne[1])
                result = difficulty.ToString() + "_1_3";
            if ((areaNumber - 69) == Variables.actOne[2] || (areaNumber - 69 * 2) == Variables.actOne[2] || areaNumber == Variables.actOne[2])
                result = difficulty.ToString() + "_1_4_1";
            if ((areaNumber - 69) == Variables.actOne[3] || (areaNumber - 69 * 2) == Variables.actOne[3] || areaNumber == Variables.actOne[3])
                result = difficulty.ToString() + "_1_5";
            if ((areaNumber - 69) == Variables.actOne[4] || (areaNumber - 69 * 2) == Variables.actOne[4] || areaNumber == Variables.actOne[4])
                result = difficulty.ToString() + "_1_6";
            if ((areaNumber - 69) == Variables.actOne[5] || (areaNumber - 69 * 2) == Variables.actOne[5] || areaNumber == Variables.actOne[5])
                result = difficulty.ToString() + "_1_7_1";
            if ((areaNumber - 69) == Variables.actOne[6] || (areaNumber - 69 * 2) == Variables.actOne[6] || areaNumber == Variables.actOne[6])
                result = difficulty.ToString() + "_1_8";
            if ((areaNumber - 69) == Variables.actOne[7] || (areaNumber - 69 * 2) == Variables.actOne[7] || areaNumber == Variables.actOne[7])
                result = difficulty.ToString() + "_1_9";
            if ((areaNumber - 69) == Variables.actOne[8] || (areaNumber - 69 * 2) == Variables.actOne[8] || areaNumber == Variables.actOne[8])
                result = difficulty.ToString() + "_1_11_1";
            if ((areaNumber - 69) == Variables.actTwo[0] || (areaNumber - 69 * 2) == Variables.actTwo[0] || areaNumber == Variables.actTwo[0])
                result = difficulty.ToString() + "_2_town";
            if ((areaNumber - 69) == Variables.actTwo[1] || (areaNumber - 69 * 2) == Variables.actTwo[1] || areaNumber == Variables.actTwo[1])
                result = difficulty.ToString() + "_2_1";
            if ((areaNumber - 69) == Variables.actTwo[2] || (areaNumber - 69 * 2) == Variables.actTwo[2] || areaNumber == Variables.actTwo[2])
                result = difficulty.ToString() + "_2_3";
            if ((areaNumber - 69) == Variables.actTwo[3] || (areaNumber - 69 * 2) == Variables.actTwo[3] || areaNumber == Variables.actTwo[3])
                result = difficulty.ToString() + "_2_5_1";
            if ((areaNumber - 69) == Variables.actTwo[4] || (areaNumber - 69 * 2) == Variables.actTwo[4] || areaNumber == Variables.actTwo[4])
                result = difficulty.ToString() + "_2_6_2";
            if ((areaNumber - 69) == Variables.actTwo[5] || (areaNumber - 69 * 2) == Variables.actTwo[5] || areaNumber == Variables.actTwo[5])
                result = difficulty.ToString() + "_2_4";
            if ((areaNumber - 69) == Variables.actTwo[6] || (areaNumber - 69 * 2) == Variables.actTwo[6] || areaNumber == Variables.actTwo[6])
                result = difficulty.ToString() + "_2_8";
            if ((areaNumber - 69) == Variables.actTwo[7] || (areaNumber - 69 * 2) == Variables.actTwo[7] || areaNumber == Variables.actTwo[7])
                result = difficulty.ToString() + "_2_9";
            if ((areaNumber - 69) == Variables.actTwo[8] || (areaNumber - 69 * 2) == Variables.actTwo[8] || areaNumber == Variables.actTwo[8])
                result = difficulty.ToString() + "_2_11_1";
            if ((areaNumber - 69) == Variables.actTwo[9] || (areaNumber - 69 * 2) == Variables.actTwo[9] || areaNumber == Variables.actTwo[9])
                result = difficulty.ToString() + "_2_12";
            if ((areaNumber - 69) == Variables.actTwo[10] || (areaNumber - 69 * 2) == Variables.actTwo[10] || areaNumber == Variables.actTwo[10])
                result = difficulty.ToString() + "_2_14_2";
            if ((areaNumber - 69) == Variables.actThree[0] || (areaNumber - 69 * 2) == Variables.actThree[0] || areaNumber == Variables.actThree[0])
                result = difficulty.ToString() + "_3_town";
            if ((areaNumber - 69) == Variables.actThree[1] || (areaNumber - 69 * 2) == Variables.actThree[1] || areaNumber == Variables.actThree[1])
                result = difficulty.ToString() + "_3_1";
            if ((areaNumber - 69) == Variables.actThree[2] || (areaNumber - 69 * 2) == Variables.actThree[2] || areaNumber == Variables.actThree[2])
                result = difficulty.ToString() + "_3_3_1";
            if ((areaNumber - 69) == Variables.actThree[3] || (areaNumber - 69 * 2) == Variables.actThree[3] || areaNumber == Variables.actThree[3])
                result = difficulty.ToString() + "_3_10_1";
            if ((areaNumber - 69) == Variables.actThree[4] || (areaNumber - 69 * 2) == Variables.actThree[4] || areaNumber == Variables.actThree[4])
                result = difficulty.ToString() + "_3_5";
            if ((areaNumber - 69) == Variables.actThree[5] || (areaNumber - 69 * 2) == Variables.actThree[5] || areaNumber == Variables.actThree[5])
                result = difficulty.ToString() + "_3_7";
            if ((areaNumber - 69) == Variables.actThree[6] || (areaNumber - 69 * 2) == Variables.actThree[6] || areaNumber == Variables.actThree[6])
                result = difficulty.ToString() + "_3_8_2";
            if ((areaNumber - 69) == Variables.actThree[7] || (areaNumber - 69 * 2) == Variables.actThree[7] || areaNumber == Variables.actThree[7])
                result = difficulty.ToString() + "_3_9";
            if ((areaNumber - 69) == Variables.actThree[8] || (areaNumber - 69 * 2) == Variables.actThree[8] || areaNumber == Variables.actThree[8])
                result = difficulty.ToString() + "_3_13";
            if ((areaNumber - 69) == Variables.actThree[9] || (areaNumber - 69 * 2) == Variables.actThree[9] || areaNumber == Variables.actThree[9])
                result = difficulty.ToString() + "_3_14_2";
            if ((areaNumber - 69) == Variables.actThree[10] || (areaNumber - 69 * 2) == Variables.actThree[10] || areaNumber == Variables.actThree[10])
                result = difficulty.ToString() + "_3_15";
            if ((areaNumber - 69) == Variables.actThree[11] || (areaNumber - 69 * 2) == Variables.actThree[11] || areaNumber == Variables.actThree[11])
                result = difficulty.ToString() + "_3_17_1";
            if ((areaNumber - 69) == Variables.actThree[12] || (areaNumber - 69 * 2) == Variables.actThree[12] || areaNumber == Variables.actThree[12])
                result = difficulty.ToString() + "_3_18_1";

            return result;
        }
        #endregion
        #region GetAreaRealName
        /// <summary>
        /// returns the Areaname
        /// </summary>
        /// <param name="areaId">AreaId String e.x. 1_2_town</param>
        /// <returns>Areaname e.x. Lioneye's Watch</returns>
        public static String GetAreaRealName(String areaId)
        {
            String _substring = areaId.Substring(1);
            switch (_substring)
            {
                case "_1_town":
                    return "Lioneye's Watch";
                case "_1_2":
                    return "The Coast";
                case "_1_3":
                    return "The Mud Flats";
                case "_1_3_a":
                    return "The Fetid Pool";
                case "_1_4_0":
                    return "The Flooded Depths";
                case "_1_4_1":
                    return "The Lower Submerged Passage";
                case "_1_4_2":
                    return "The Upper Submerged Passage";
                case "_1_5":
                    return "The Ledge";
                case "_1_6":
                    return "The Climb";
                case "_1_7_1":
                    return "The Lower Prison";
                case "_1_7_2":
                    return "The Upper Prison";
                case "_1_8":
                    return "Prisoner's Gate";
                case "_1_9":
                    return "The Ship Graveyard";
                case "_1_9a":
                    return "The Ship Graveyard Cave";
                case "_1_10":
                    return "The Coves";
                case "_1_11_1":
                    return "The Cavern of Wrath";
                case "_1_11_2":
                    return "The Cavern of Anger";
            }
            return "";
        }
        #endregion
    }
}
