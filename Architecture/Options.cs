using System.Linq;
using Json;

namespace LabirintDemoGame.Architecture
{
    public class Option : PlotParameters
    {
        public string Name { get; set; }
        public int[] Requirements { get; set; }
        public string Result { get; set; }

        public bool IsValid(Player player)
        {
            return player.Check()
                .Select((x, index) => x >= Requirements[index])
                .All(x => x);
        }

        public override string ToString()
        {
            return $"{Result}";
        }

        public static Option FromJson(string json)
        {
            return JsonParser.Deserialize<Option>(json);
        }
    }
}