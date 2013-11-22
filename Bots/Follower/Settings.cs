using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Loki;
using Loki.Bot;
using Loki.Utilities;
using Loki.Utilities.Misc;
using Newtonsoft.Json;
namespace Follower
{
    class Settings
    {
        public String leaderName { get; set; }
        public float lootDistance { get; set; }
        public float fightDistance { get; set; }
        public float leaderDistance { get; set; }
        public Int32 chickenHealthPoints = CharacterSettings.Instance.ChickenHealthPercent;
        public Int32 difficulty { get; set; }

        public static Settings Instance = Initialize();
        private static Settings CreateDefaultSettings()
        {
            Settings result = new Settings();
            result.chickenHealthPoints = CharacterSettings.Instance.ChickenHealthPercent;
            result.leaderDistance = 50f;
            result.leaderName = "LEADER";
            result.lootDistance = 60f;
            result.fightDistance = 60f;
            result.difficulty = 1;
            StreamWriter writer = new StreamWriter((Loki.Bot.GlobalSettings.SettingsPath + "\\Follower\\" + Loki.Game.LokiPoe.Me.Name + ".cfg"));
            writer.Write(JsonConvert.SerializeObject(result));
            writer.Close();
            return result;
        }
        public static void Save()
        {
            StreamWriter writer = new StreamWriter((Loki.Bot.GlobalSettings.SettingsPath + "\\Follower\\" + Loki.Game.LokiPoe.Me.Name + ".cfg"));
            writer.Write(JsonConvert.SerializeObject(Instance));
            writer.Close();
        }
        public static Settings Initialize()
        {
            Settings result = null;
            if (System.IO.File.Exists(Loki.Bot.GlobalSettings.SettingsPath + "\\Follower\\" + Loki.Game.LokiPoe.Me.Name + ".cfg"))
            {
                result = JsonConvert.DeserializeObject<Settings>(System.IO.File.ReadAllText(Loki.Bot.GlobalSettings.SettingsPath + "\\Follower\\" + Loki.Game.LokiPoe.Me.Name + ".cfg"));
            }
            else
            {
                result = CreateDefaultSettings();
            }
            return result;
        }
    }
    
}
