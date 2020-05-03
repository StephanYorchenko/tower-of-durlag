using System;
using System.Collections.Generic;
using System.Linq;

namespace LabirintDemoGame
{
    public class Player
    {
        public List<PlotSubject> Bag;
        public int Health { get; private set; }

        public Player()
        {
            Bag = new List<PlotSubject>();
            Health = 100;
        }

        public Player(int health, IEnumerable<PlotSubject> bag)
        {
            Bag = bag.ToList();
            Health = health;
        }

        public void ChangeHp(int deltaHp)
        {
            Health -= deltaHp;
        }

        public static Direction Move(Directions direction)
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