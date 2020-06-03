using System;
using System.Collections.Generic;

namespace LabirintDemoGame.Architecture
{
    public class Player : PlotParameters
    {
        public Player(int health = 100,
            int torch = 1,
            int bandage = 1,
            int herb = 1,
            int sword = 1,
            int gold = 0,
            int sup = 1)
        {
            Hp = health;
            Torch = torch;
            Bandage = bandage;
            Herb = herb;
            Sword = sword;
            Gold = gold;
            Supplies = sup;
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
            Minimize();
        }

        private void Minimize()
        {
            Hp = Math.Max(0, Hp);
            Torch = Math.Max(0, Torch);
            Bandage = Math.Max(0, Bandage);
            Herb = Math.Max(0, Herb);
            Gold = Math.Max(Gold, 0);
            Supplies = Math.Max(Supplies, 0);
        }

        public static Direction Move(Directions direction)
        {
            return Direction.Create(direction);
        }

        public override string ToString()
        {
            return $"{Math.Max(0, Hp)}";
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
