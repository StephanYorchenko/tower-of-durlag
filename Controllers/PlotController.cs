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
            var rnd = new Random();
            Depth = rnd.Next(3, 10);
            Location = "TowerOfDurlag";
            CreateAdventure();
            if (!config)
                SetNextAct();
        }
        public string Location { get; set; }
        public int Depth { get; set; }

        public Queue<string> Adventure { get; set; }

        public Option[] CurrentOptions => CurrentAct.GetOptions();
        private IEnumerable<string> Directory => Config.GetConfig(Location);

        public void SetNextAct()
        {
            if (Adventure.Count == 0)
                CreateAdventure();
            else
                SetNextActFromJson(Adventure.Dequeue());
        }

        private void SetNextActFromJson(string json)
        {
            CurrentAct = PlotAct.CreateFromJson(json);
        }

        private void CreateAdventure()
        {
            Adventure = new Queue<string>();
            var temp = Config.GetConfig(Location);
            var rnd = new Random();
            temp = temp.OrderBy(x => rnd.Next()).ToList();
            foreach (var item in temp)
            {
                Adventure.Enqueue(item);
            }
        }
    }
}