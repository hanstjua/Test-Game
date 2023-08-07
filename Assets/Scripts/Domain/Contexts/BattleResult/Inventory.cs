using System;
using System.Collections.Generic;
using Battle.Common;

namespace BattleResult
{
    public class Inventory : ValueObject<Dictionary<Item, int>>
    {
        public Inventory
        (
            Dictionary<Item, int> content
        )
        {
            Content = content;
        }

        public override Dictionary<Item, int> Value()
        {
            return Content;
        }

        public Dictionary<Item, int> Content { get; private set; }

        public Inventory Add(Item item, int quantity)
        {
            if (Content.ContainsKey(item))
            {
                Content[item] = Math.Min(Content[item] + quantity, 99);
            }
            else
            {
                Content.Add(item, quantity);
            }

            return this;
        }
    }
}