using System.IO;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using TShockAPI;

namespace ShakePlugin
{
    public class Config
    {
        [JsonProperty("Shake Cooldown")]
        public static int ShakeCooldown { get; set; } = 30;
        public static string[] RankList { get; set; } = { "User" };
        [JsonIgnore]
        private static readonly string Path = System.IO.Path.Combine(TShock.SavePath, "ShakePlugin.json");
        public static Config Create()
        {
            if (File.Exists(Path))
                return Read();
            TShock.Log.Info("Creating new Config file");
            try
            {
                var conf = new Config();
                File.WriteAllText(Path, JsonConvert.SerializeObject(conf, Formatting.Indented));
                return conf;

            }
            catch
            {
                TShock.Log.ConsoleError("[ShakePlugin] Failed to create new config file!");
            }
            return new Config();
        }
        public static Config Read(bool returnNull = false)
        {
            TShock.Log.Info("Reading config file");
            try
            {
                var sr = new StreamReader(File.Open(Path, FileMode.Open));
                var rawJson = sr.ReadToEnd();
                sr.Close();

                var conf = JsonConvert.DeserializeObject<Config>(rawJson);
                File.WriteAllText(Path, JsonConvert.SerializeObject(conf, Formatting.Indented));
                return conf;




            }
            catch (JsonReaderException e)
            {
                TShock.Log.Error(e.ToString());
                var additional = returnNull ? "" : " Using Defaults!";
                TShock.Log.ConsoleError($"[ShakePlugin] Failed to load config!");
                return returnNull ? null : new Config();

            }
        }



        public class ConfigColor
        {
            public byte R;
            public byte G;
            public byte B;
            public Color CovertToColor() => new Color(R, G, B, 255);

        }

        public class OldConfig
        {
            [JsonProperty("ShakePlugin")]
            public string[] RankList { get; set; }

        }
    }
}





