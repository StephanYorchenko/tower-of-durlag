using System;
using System.Collections.Generic;
using System.Linq;
using LabirintDemoGame.Architecture;

namespace LabirintDemoGame.Controllers
{
    public class PlotController
    {
        public PlotAct CurrentAct { get; private set; }

        public PlotController(bool config = false)
        {
            if (!config)
                SetNextAct();
        }
        public Option[] CurrentOptions => CurrentAct.GetOptions();
        private static IEnumerable<string> Directory => Config.Cards;

        public void SetNextAct()
        {
            var random = new Random();
            var jsonTemplate = Directory.ElementAt(random.Next(0, Directory.Count() - 1));
            SetNextActFromJson(jsonTemplate);
        }

        private void SetNextActFromJson(string json)
        {
            CurrentAct = PlotAct.CreateFromJson(json);
        }
    }
}