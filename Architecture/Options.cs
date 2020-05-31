using System.Linq;
using Json;

namespace LabirintDemoGame.Architecture
{
    public class Option : PlotParameters
    {
        public string Name { get; set; }
        public string Requirements { get; set; }
        public string Result { get; set; }

        public bool IsValid(Player player)
        {
            var requirements = Requirements.Split(',')
                .Select(x=> int.Parse(x))
                .ToList();
            return player.Check()
                .Select((x, index) => x >= requirements[index])
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