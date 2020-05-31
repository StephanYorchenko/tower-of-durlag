using System;
using System.Collections.Generic;

namespace LabirintDemoGame.Architecture
{
    public class Player : PlotParameters
    {
        public Player(int health = 1)
        {
            Hp = health;
            Torch = 1;
            Bandage = 1;
            Herb = 1;
            Sword = 1;
            Gold = 0;
            Supplies = 1;
        }

        public void ApplyChanges(Option changes)
        {
            Hp = Math.Min(100, Hp + changes.Hp);
            Torch += changes.Torch;
            Bandage += changes.Bandage;
            Herb += changes.Herb;
            Sword = changes.Sword != 0? changes.Sword : Sword;
            Gold += changes.Gold;
            Supplies += changes.Supplies;
        }

        public static Direction Move(Directions direction)
        {
            return Direction.Create(direction);
        }

        public override string ToString()
        {
            return $"{Hp}";
        }

        public IEnumerable<int> Check()
        {
            return new List<int>
            {
                Torch,
                Bandage,
                Herb,
                Sword,
                Gold,
                Supplies
            };
        }

        public bool IsDead() => Hp <= 0;
    }
}