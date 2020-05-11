using System.Collections.Generic;
using System.Linq;

namespace LabirintDemoGame.Architecture
{
    public class PlotAct
    {
        public string Text;
        public List<PlotSubject> Options;

        public PlotAct(string text, IEnumerable<PlotSubject> options)
        {
            Text = text;
            Options = options.ToList();
        }
    }
}