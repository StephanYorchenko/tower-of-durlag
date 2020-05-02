using System;
using System.Collections.Generic;
using System.Linq;

namespace LabirintDemoGame
{
    public class Player
    {
        public Cell Position;
        public List<PlotSubject> Bag;
        public int Health;

        public Player(Cell position)
        {
            Position = position;
            Bag = new List<PlotSubject>();
            Health = 100;
        }

        public void Move(Directions direction)
        {
            switch (direction)
            {
                case Directions.Up:
                    Position.Y--;
                    break;
                case Directions.Down:
                    Position.Y++;
                    break;
                case Directions.Left:
                    Position.X--;
                    break;
                case Directions.Right:
                    Position.X++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public override string ToString()
        {
            var bag = Bag.Select(x => x.ToString());
            return $"{Position.X} -- {Position.Y} -- {Health} -- <{string.Join("/", bag)}>";
        }
    }
}