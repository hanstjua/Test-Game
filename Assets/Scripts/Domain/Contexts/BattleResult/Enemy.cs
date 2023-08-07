using System;
using System.Collections.Generic;

namespace BattleResult
{
    public class Enemy : Entity
    {
        private string _id;

        public Enemy(
            string id,
            List<Loot> loots,
            int money
        )
        {
            _id = id;
            Loots = loots;
            Money = money;
        }

        public override object Id()
        {
            return _id;
        }

        public List<Loot> Loots { get; private set; }

        public int Money { get; private set; }
    }
}