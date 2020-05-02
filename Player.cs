using System;
using System.Collections.Generic;
using System.Linq;

namespace LabirintDemoGame
{
    public class Player
    {
        public List<PlotSubject> Bag;
        public int Health;

        public Player()
        {
            Bag = new List<PlotSubject>();
            Health = 100;
        }

        public Direction Move(Directions direction)
        {
            return Direction.Create(direction);
        }

        public override string ToString()
        {
            var bag = Bag.Select(x => x.ToString());
            return $"{Health} -- <{string.Join("/", bag)}>";
        }
    }
}