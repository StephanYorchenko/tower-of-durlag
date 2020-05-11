namespace LabirintDemoGame.Architecture
{
    public class PlotSubject
    {
        public string Name { get; }

        public PlotSubject(string name) { Name = name; }

        public override string ToString() => Name;
    }
}