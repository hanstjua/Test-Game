using System;
using Battle;

namespace BattleResult
{
    public class Loot : ValueObject<(Item, double)>
    {
        public Loot(
            Item item,
            double chance
        )
        {
            Item = item;
            Chance = chance;
        }

        public override (Item, double) Value()
        {
            return (Item, Chance);
        }

        public Item Item { get; private set; }
        public double Chance { get; private set; }
    }
}