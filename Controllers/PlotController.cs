using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LabirintDemoGame.Architecture;

namespace LabirintDemoGame.Controllers
{
    public class PlotController
    {
        public PlotAct CurrentAct { get; private set; }

        public PlotController()
        {
            SetNextAct();
        }
        public Option[] CurrentOptions => CurrentAct.GetOptions();
        private static IEnumerable<string> directory => Config.Cards;

        public void SetNextAct()
        {
            var random = new Random();
            var jsonTemplate = directory.ElementAt(random.Next(0, directory.Count() - 1));
            CurrentAct = PlotAct.CreateFromJson(jsonTemplate);
        }

        public bool EndPlotStep()
        {
            return false;
        }
    }
}