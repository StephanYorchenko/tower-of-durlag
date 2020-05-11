using LabirintDemoGame.Controllers;

namespace LabirintDemoGame.Architecture
{
    public class Option : PlotAction
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

        public bool IsValid(Player player)
        {
            return true;
        }
    }
}