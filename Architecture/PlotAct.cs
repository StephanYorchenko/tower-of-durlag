using System.IO;
using Json;

namespace LabirintDemoGame.Architecture
{
    public class PlotAct
    {
        public string Text { get; set; }
        public string Image { get; set; }
        private Option option1;
        private Option option2;
        private Option autoOption;

        public static PlotAct CreateFromJson(string fileName)
        {
            var file = $@"Plot/Cards/{fileName}.json";

             var act = JsonParser.Deserialize<PlotAct>(ReadJson(file));
             act.option1 = Option.FromJson(ReadJson($@"Plot/Options/{fileName}1.json"));
             act.option2 = Option.FromJson(ReadJson($@"Plot/Options/{fileName}2.json"));
             act.autoOption = Option.FromJson(ReadJson($@"Plot/Options/{fileName}0.json"));
            
            return act;
        }

        public Option GetAction() => autoOption;
        
        public Option[] GetOptions() => new[]{option1, option2};

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