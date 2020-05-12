using System.IO;
using System.Linq;
using Json;

namespace LabirintDemoGame.Architecture
{
    public class Option : PlotParameters
    {
        public string Name;
#pragma warning disable 649
        public int[] requirements;
#pragma warning restore 649
        public string Result;

        public bool IsValid(Player player)
        {
            return player.Check()
                .Select((x, index) => x >= requirements[index])
                .All(x => x);
        }

        public override string ToString()
        {
            return $"{Name} -- {Result}";
        }

        public static Option FromJson(string fileName)
        {
            if (File.Exists(fileName))
                return JsonParser.Deserialize<Option>(File.ReadAllText(fileName));
            
            throw new FileNotFoundException();
        }
    }
}