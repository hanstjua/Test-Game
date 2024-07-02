using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public class Item
    {
        public string Name { get; private set; }

        public Item(string name) { Name = name; }

        public static readonly Item Potion = new("Potion");
        public static readonly Item Remedy = new("Remedy");
        public static readonly Item Ether = new("Ether");
        public static readonly Item Grenade = new("Grenade");
    }

    public class ItemFactory
    {
        public static readonly Dictionary<string, Item> Instances =
        typeof(Item)
        .GetFields()
        .Where(f => f.FieldType == typeof(Item))
        .ToDictionary(f => ((Item)f.GetValue(0)).Name, f => (Item) f.GetValue(0));
    }
}