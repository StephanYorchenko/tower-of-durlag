namespace LabirintDemoGame
{
    public struct Cell
    {
        public int X { get; }
        public int Y { get; }
        private CellTypes Type { get; }

        public Cell(int x, int y, CellTypes type)
        {
            X = x;
            Y = y;
            Type = type;
        }

        public override string ToString()
        {
            return $"X:{X} Y:{Y} Type:{Type}";
        }
    }
}