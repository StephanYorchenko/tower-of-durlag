using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LabirintDemoGame.Architecture
{
    public class Leaderboard
    {
        public List<string> Leaders;

        public Leaderboard()
        {
            Leaders = ReadFile("leaders.txt").Split('\n').ToList();
        }

        private void ToFile()
        {
            using (var w = new StreamWriter("leaders.txt"))
                w.Write(ToString());
        }

        private static string ReadFile(string fileName)
        {
            using (var r = new StreamReader(fileName))
                return r.ReadToEnd();
        }

        public override string ToString()
        {
            return string.Join("\n", Leaders);
        }

        public void Update(string name, int score)
        {
            Leaders.Add(name + $" {score}");
            Leaders = Leaders.Select(x => x.Split(' '))
                .Select(x => Tuple.Create(x[0], int.Parse(x[1])))
                .OrderBy(x => x.Item2)
                .Reverse()
                .Select(x => x.Item1 + $" {x.Item2}")
                .Take(5)
                .ToList();
            ToFile();
        }

        public IEnumerable<Tuple<string, string>> Show()
        {
            return Leaders.Select(record => record.Split(' ')
                .ToArray())
                .Select(c => Tuple.Create(c[0], c[1]));
        }
    }
}