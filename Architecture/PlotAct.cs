using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace LabirintDemoGame.Architecture
{
    public class PlotAct
    {
        public string Text;
        public string Image;
        private readonly Option Option1;
        private readonly Option Option2;
        private readonly PlotAction PlotAction;

        public PlotAct(PlotAction plotAction, Option option1, Option option2)
        {
            PlotAction = plotAction;
            Option1 = option1;
            Option2 = option2;
        }

        public static PlotAct CreateFromJson(string fileName)
        {
            if (File.Exists(fileName))
            {
                return JsonConvert.DeserializeObject<PlotAct>(File.ReadAllText(fileName));
            }
            throw new FileNotFoundException();
        }

        public PlotAction GetAction() => PlotAction;
        
        public Option[] GetOptions() => new Option[]{Option1, Option2};
    }
}