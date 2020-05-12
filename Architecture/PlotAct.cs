using System.IO;
using Json;

namespace LabirintDemoGame.Architecture
{
    public class PlotAct
    {
        public string Text;
        public string Image;
        private Option option1;
        private Option option2;
        private Option autoOption;

        public PlotAct(Option autoOption, Option option1, Option option2)
        {
            this.autoOption = autoOption;
            this.option1 = option1;
            this.option2 = option2;
        }

        public static PlotAct CreateFromJson(string fileName)
        {
            if (!File.Exists(fileName)) 
                throw new FileNotFoundException();
            
            var act = JsonParser.Deserialize<PlotAct>(File.ReadAllText(fileName));
            act.option1 = Option.FromJson(fileName+"1.json");
            act.option2 = Option.FromJson(fileName + "2.json");
            act.autoOption = Option.FromJson(fileName + "2.json");
            
            return act;
        }

        public PlotParameters GetAction() => autoOption;
        
        public Option[] GetOptions() => new[]{option1, option2};
    }
}