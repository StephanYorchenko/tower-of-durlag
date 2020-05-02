using System.Linq;

namespace LabirintDemoGame
{
    public class Level
    {
        public MapController Map;
        public PlotAct PlotAct;

        public Level(MapController map, PlotAct plotAct)
        {
            Map = map;
            PlotAct = plotAct;
        }

        public static Level CreateFromConfig(string text)
        {
            var parameters = text.Split('%').ToList();
            var height = int.Parse(parameters[0]);
            var width = int.Parse(parameters[1]);
            var plotText = parameters[2];
            var plotItems = parameters.Skip(3).Select(x => new PlotSubject(x));
            
            var map = new MapController(width, height);
            var plot = new PlotAct(plotText, plotItems);
    
            return new Level(map, plot);
        }
    }
}