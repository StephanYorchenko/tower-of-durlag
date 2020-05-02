namespace LabirintDemoGame
{
    public struct Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public CellTypes Type { get; }
        public bool IsExplored { get; set; }

        public Cell(int x, int y, CellTypes type)
        {
            X = x;
            Y = y;
            Type = type;
            IsExplored = true;
        }

        public bool Equals(Cell obj)
        {
            return X == obj.X && Y == obj.Y;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case CellTypes.Player:
                    return "@ ";
                case CellTypes.Empty:
                    return "  ";
                case CellTypes.End:
                    return "E ";
                case CellTypes.Start:
                    return "S ";
                case CellTypes.Wall:
                    return "# ";
                default:
                    return "??";
            }
        }
    }
}