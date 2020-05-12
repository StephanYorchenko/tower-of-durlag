using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LabirintDemoGame.Architecture;

namespace LabirintDemoGame.Controllers
{
    public class PlotController
    {
        private PlotAct currentAct;

        public PlotController()
        {
            SetNextAct();
        }
        public Option[] CurrentOptions => currentAct.GetOptions();
        private static IEnumerable<string> directory => Config.Cards;

        public void SetNextAct()
        {
            var random = new Random();
            var jsonTemplate = directory.ElementAt(random.Next(0, directory.Count() - 1));
            currentAct = PlotAct.CreateFromJson(jsonTemplate);
        }
    }
}