using System;
using System.Collections.Generic;
using Battle.Common;

namespace BattleResult
{
    public class Player : Entity
    {
        private string _id;
        public Player(
            string id,
            Inventory inventory,
            int money
        )
        {
            _id = id;
            Inventory = inventory;
            Money = money;
        }

        public override object Id()
        {
            return _id;
        }

        private Inventory Inventory { get; set; }

        private int Money { get; set; }

        public Player Store(Dictionary<Item, int> loots)
        {
            foreach(var item in loots)
            {
                Inventory.Add(item.Key, item.Value);
            }

            return this;
        }

        public Player Store(int money)
        {
            Money += money;

            return this;
        }
    }
}