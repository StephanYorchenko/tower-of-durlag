using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace LabirintDemoGame.Architecture
{
    public class Option
    {
        public string Name;
        public int Torch;
        public int Bandage;
        public int Herb;
        public bool Sword;
        public int Gold;
        public int Supplies;
        public int HP;
        public string Result;
    }
    
    public class PlotAction
    {
        public string Name;
        public int Torch;
        public int Bandage;
        public int Herb;
        public bool Sword;
        public int Gold;
        public int Supplies;
        public int HP;
        public string Result;
    }
    
    public class PlotAct
    {
        public string Text;
        public string Image;
        private Option Option1;
        private Option Option2;
        private PlotAction PlotAction;

        public static PlotAct CreateFromJson(string fileName)
        {
            if (File.Exists(fileName))
            {
                return JsonConvert.DeserializeObject<PlotAct>(File.ReadAllText(fileName));
            }
            throw new Exception("file not fond");
        }

        public PlotAction GetAction() => PlotAction;
        
        public Option[] GetOptions() => new Option[]{Option1, Option2};
    }
}