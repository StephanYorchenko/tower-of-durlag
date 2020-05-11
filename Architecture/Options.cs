using LabirintDemoGame.Controllers;

namespace LabirintDemoGame.Architecture
{
    public class Option : PlotAction
    {
        public bool IsValid(Player player)
        {
            return true;
        }
    }
}