namespace LabirintDemoGame
{
    public class PlotSubject
    {
        public string Name { get; }
        public int Count;
        
        public PlotSubject(string name)
        {
            Name = name;
            Count = 1;
        }
        
        public PlotSubject(string name, int count)
        {
            Name = name;
            Count = count;
        }

        public override string ToString()
        {
            return $"{Name} -- {Count}";
        }
    }
}