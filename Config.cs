using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Json;
using LabirintDemoGame.Architecture;

namespace LabirintDemoGame
{
    public class SetAreas
    {
        public List<object> TowerOfDurlag { get; set; }
        public List<object> Larswood { get; set; }
        public List<object> FirewineRuins { get; set; }
        public List<object> Underdark { get; set; }

        public static List<string> ConvertObjects(List<object> input)
        {
            return input.Select(o => o.ToString()).ToList();
        }
        public List<string> GetLocations(string area)
        {
            
            switch (area)
            {
                case "TowerOfDurlag":
                    return ConvertObjects(TowerOfDurlag);
                case "Larswood":
                    return ConvertObjects(Larswood);
                case "FirewineRuins":
                    return ConvertObjects(FirewineRuins);
                case "Underdark":
                    return ConvertObjects(Underdark);
                default:
                    throw new ArgumentException();
            }
        }
    }
    public static class Config
    {

        public static List<string> GetConfig(string location)
        {
            var config = JsonParser.Deserialize<SetAreas>
                                        (ReadJson(@"Plot/Config.json"));
            return config.GetLocations(location);
        }
        
        private static string ReadJson(string fileName)
        {
            using (var r = new StreamReader(fileName))
            {
                var json = r.ReadToEnd();
                return json;
            }
        }
    }
}