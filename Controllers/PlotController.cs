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
        private static IEnumerable<string> Directory => Config.Cards;

        public void SetNextAct()
        {
            var random = new Random();
            var jsonTemplate = Directory.ElementAt(random.Next(0, Directory.Count() - 1));
            CurrentAct = PlotAct.CreateFromJson(jsonTemplate);
        }

        public bool EndPlotStep()
        {
            return false;
        }
    }
}