using System;

namespace LabirintDemoGame
{
    public class Direction
    {
        public int X;
        public int Y;

        public Direction(int x, int y)
        {
            X = x;
            Y = y;
        }
        
        public static Direction Create(Directions direction)
        {
            switch (direction)
            {
                case Directions.Up:
                    return new Direction(0, -1);
                case Directions.Down:
                    return new Direction(0, 1);
                case Directions.Left:
                    return new Direction(-1, 0);
                case Directions.Right:
                    return new Direction(1, 0);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}