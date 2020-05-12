using System.Linq;

namespace LabirintDemoGame.Controllers
{
    public class Level
    {
        public readonly MapController Map;
        public PlotController Plot;

        public Level(MapController map, PlotController plot)
        {
            Map = map;
            Plot = plot;
        }

        public static Level CreateFromConfig(string text)
        {
            var parameters = text.Split('%').ToList();
            var height = int.Parse(parameters[0]);
            var width = int.Parse(parameters[1]);
            var map = new MapController(width, height);
            var plot = new PlotController(true);
            return new Level(map, plot);
        }
    }
}