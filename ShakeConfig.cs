using System.IO;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using TshockAPI;

namespace ShakePlugin
{
    public class Config
    {
        [JsonIgnore]
        private static readonly string Path = System.IO.Path.Combine(Tshock.SavePath, "ShakePlugin.json");
        public static Config Create()
        {
            if (File.Exists(Path))
                return Read();
                Tshock.Log.Info("Creating new Config file");
                try
                {
                    var conf = new Config();
                    File.WriteAllText(Path,JsonConvert.SerializeObject(conf, Formatting.Indented));
                    return conf;

                }
                catch
                {
                    Tshock.Log.ConsoleError("[ShakePlugin] Failed to create new config file!");
                }
        }
        public static Config Read(bool returnNull = false)
        {
            Tshock.Log.Info("Reading config file");
            try
            {
                var sr = new StreamReader(File.Open(Path,FileMode.Open));
                var rawJson = sr.ReadToEnd();
                sr.close();
                if (rawJson.Contains("Cooldown"))
                {
                    var conf = JsonConvert.DeseralizeObject<Config>(rawJson);
                    File.WriteAllText(Path, JsonConvert.SerializeObject(conf, Formatting.Indented));
                    return conf;
                }
                Tshock.Log.Info("Converting old config file");
                var old = JsonConvert.DeseralizeObject<OldConfig>(rawJson);
                var newConfig = new Config
                {
                    ShakeCooldown = old.Shake.cd
                };

                File.WriteAllText(Path, JsonConvert.SerializeObject(newConfig, Formatting.Indented));
                return newConfig;
            }
            catch (JsonReaderException e)
        {
            Tshock.Log.Error(e.ToString());
            var additional = returnNull ? "" : " Using Defaults!";
            Tshock.Log.ConsoleError($"[ShakePlugin] Failed to load config!");
            return returnNull ? null : new Config();
        
        }

        public Config()
        {
            RankList = new [] {"Owner, Administrator, Moderator, Helper, Retired, User+, User,"};
            ShakeCooldown = 30;
        }
        [JsonProperty("Shake Cooldown")]
        public string[] Shake Cooldown{get; set; }

        public class ConfigColor
        {
            public byte R;
            public byte G;
            public byte B;
            public Color CovertToColor()=> new Color(R, G, B, 255);
            
        }

        public class OldConfig
        {
            [JsonProperty("ShakePlugin")]
            public string[] RankList {get; set; }

        }
                    


        
