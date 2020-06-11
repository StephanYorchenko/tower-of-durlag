using System;
using System.Collections.Generic;
using System.Linq;
using LabirintDemoGame.Architecture;

namespace LabirintDemoGame.Controllers
{
    public class PlotController
    {
        public PlotAct CurrentAct { get; private set; }

        public PlotController()
        {
            Location = "TowerOfDurlag";
            CreateAdventure();
        }
        public string Location { get; set; }
        public int Depth { get; set; }
        public bool Flag { get; set; }

        public Queue<string> Adventure { get; set; }

        public Option[] CurrentOptions => CurrentAct.GetOptions();
        private IEnumerable<string> Directory => Config.GetConfig(Location);

        public void SetNextAct()
        {
            if (Adventure.Count > 0)
                SetNextActFromJson(Adventure.Dequeue());
            else
                Flag = true;
        }

        private void SetNextActFromJson(string json) 
        {
            CurrentAct = PlotAct.CreateFromJson(json);
        }

        public void CreateAdventure()
        {
            var rnd = new Random();
            Depth = rnd.Next(3, 7);
            Adventure = new Queue<string>();
            Console.WriteLine(Adventure.Count);
            var temp = Config.GetConfig(Location);
            temp = temp.OrderBy(x => rnd.Next()).Take(Depth).ToList();
            foreach (var item in temp)
                Adventure.Enqueue(item);

            Flag = false;
            if (Adventure.Count > 0)
                SetNextAct();
        }
    }
}