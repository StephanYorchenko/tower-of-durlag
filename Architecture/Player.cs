using System;
using System.Collections.Generic;
using System.Linq;

namespace LabirintDemoGame.Architecture
{
    public class Player : PlotParameters
    {
        public Player(int health = 100)
        {
            HP = health;
            Torch = 1;
            Bandage = 1;
            Herb = 1;
            Sword = true;
            Gold = 0;
            Supplies = 1;
        }

        public void ApplyChanges(Option changes)
        {
            HP = Math.Max(100, HP + changes.HP);
            Torch += changes.Torch;
            Bandage += changes.Bandage;
            Herb += changes.Herb;
            Sword = changes.Sword;
            Gold += changes.Gold;
            Supplies += changes.Supplies;
        }

        public static Direction Move(Directions direction)
        {
            return Direction.Create(direction);
        }

        public override string ToString()
        {
            return $"{HP}";
        }

        public List<int> Check()
        {
            return new List<int>
            {
                Torch,
                Bandage,
                Herb,
                Sword ? 1 : 0,
                Gold,
                Supplies
            };
        }
    }
}