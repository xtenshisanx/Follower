using System;
using Loki;
using Loki.Bot;

namespace Follower
{
    class Settings
    {
        public static String leaderName = "BadassRange";

        public static float lootDistance = 60;
        public static float fightDistance = 60;

        public static Int32 chickenHealthPoints = CharacterSettings.Instance.ChickenHealthPercent;
        
        //Difficulties
        //1 Normal
        //2 Cruel
        //3 Merveil
        public static Int32 difficulty = 1;

    }
}
