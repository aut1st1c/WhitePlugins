using Newtonsoft.Json;
using System.IO;

namespace WhiteTax.Base
{
    public class Configuration
    {
        public int Balance { get; set; }
        public int Tax { get; set; }
    }
    public class JsonParser
    {
        public static Configuration Config { get; set; }
        public static void LoadJson()
        {
            string readConfig = File.ReadAllText(Path.Combine("Plugins", "WhiteTax.json"));
            Config = JsonConvert.DeserializeObject<Configuration>(readConfig);
        }
        public static void SaveJson()
        {
            string serializedConfig = JsonConvert.SerializeObject(Config);
            File.WriteAllText(Path.Combine("Plugins", "WhiteTax.json"), serializedConfig);
        }
    }
}