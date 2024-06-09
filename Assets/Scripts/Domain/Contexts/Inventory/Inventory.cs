using System;
using System.Collections.Generic;
using System.Linq;
using Battle;

namespace Inventory
{
    public class Inventory : Entity
    {
        public override object Id() => _id;

        private readonly Dictionary<Item, int> _items = new();
        private readonly Dictionary<Equipment, (int, int)> _equipment = new();
        private readonly InventoryId _id;

        public int Amount(Item item) => _items[item];
        public int Amount(Equipment equipment) => _equipment[equipment].Item1;
        public int Equipped(Equipment equipment) => _equipment[equipment].Item2;
        public Item[] AllItems => _items.Keys.ToArray();
        public Equipment[] AllEquipment => _equipment.Keys.ToArray();
        
        public Inventory(InventoryId id)
        {
            _id = id;
        }

        public Inventory(InventoryId id, Dictionary<Item, int> items, Dictionary<Equipment, (int, int)> equipment)
        {
            _id = id;

            foreach (var kvp in items)
            {
                if (kvp.Value < 1 || kvp.Value > 99) throw new ArgumentException($"Amount for {kvp.Key} ({kvp.Value}) is not between 1 and 99 (inclusive)!");
            }

            foreach (var kvp in equipment)
            {
                var amount = kvp.Value.Item1;
                var equipped = kvp.Value.Item2;

                if (amount < 1 || amount > 99) throw new ArgumentException($"Amount for {kvp.Key.Value()} ({amount}) is not between 1 and 99 (inclusive)!");
                if (equipped > amount) throw new ArgumentException($"Equipped for {kvp.Key.Value()} ({equipped}) is more than amount ({amount})!");
                if (equipped < 0) throw new ArgumentException($"Equipped for {kvp.Key.Value()} cannot be less than zero ({equipped})!");
            }

            _items = items;
            _equipment = equipment;
        }

        public Inventory AddItem(Item item, int amount)
        {
            if (amount < 1) throw new ArgumentException($"Amount {amount} is < 1!");

            if (!_items.Keys.Contains(item))
            {
                var newItems = _items.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                newItems[item] = amount < 100 ? amount : 99;
                return new(_id, newItems, _equipment);
            }
            else if (_items[item] < 99)
            {
                var newItems = _items.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                newItems[item] = amount + _items[item] < 100 ? amount + _items[item] : 99;
                return new(_id, newItems, _equipment);
            }
            else
            {
                return this;
            }
        }

        public Inventory RemoveItem(Item item, int amount)
        {
            if (amount < 1) throw new ArgumentException($"Amount {amount} is < 1!");

            if (!_items.Keys.Contains(item)) throw new ArgumentException($"Inventory has no {item}!");

            var newAmount = _items[item] < amount ? 0 : _items[item] - amount;
            
            if (newAmount > 0)
            {
                var newItems = _items.Where(kvp => kvp.Key != item).ToDictionary(kvp => kvp.Key, (kvp) => kvp.Value);
                return new(_id, newItems, _equipment);
            }
            else
            {
                var newItems = _items.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                newItems[item] = newAmount;
                return new(_id, newItems, _equipment);
            }
        }

        public Inventory AddEquipment(Equipment equipment, int amount)
        {
            if (amount < 1) throw new ArgumentException($"Amount {amount} is < 1!");

            if (!_equipment.Keys.Contains(equipment))
            {
                var newEquipment = _equipment.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                newEquipment[equipment] = amount < 100 ? (amount, 0) : (99, 0);
                return new(_id, _items, newEquipment);
            }
            else if (_equipment[equipment].Item1 < 99)
            {
                var newEquipment = _equipment.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                var oldAmount = newEquipment[equipment].Item1;
                var equipped = newEquipment[equipment].Item2;
                newEquipment[equipment] = amount + oldAmount < 100 ? (amount, equipped) : (99, equipped);
                return new(_id, _items, newEquipment);
            }
            else
            {
                return this;
            }
        }

        public Inventory RemoveEquipment(Equipment equipment, int amount)
        {
            if (amount < 1) throw new ArgumentException($"Amount {amount} is < 1!");

            if (!_equipment.Keys.Contains(equipment)) throw new ArgumentException($"Inventory has no {equipment.Value()}!");

            if (_equipment[equipment].Item2 > amount) throw new ArgumentException($"Equipped for {equipment.Value()} is more than removal amount {amount}!");

            var newAmount = _equipment[equipment].Item1 < amount ? 0 : _equipment[equipment].Item1 - amount;
            
            if (newAmount > 0)
            {
                var newEquipment = _equipment.Where(kvp => kvp.Key != equipment).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                return new(_id, _items, newEquipment);
            }
            else
            {
                var newEquipment = _equipment.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                var equipped = newEquipment[equipment].Item2;
                newEquipment[equipment] = (newAmount, equipped);
                return new(_id, _items, newEquipment);
            }
        }

        public Inventory Equip(Equipment equipment)
        {
            if (!_equipment.Keys.Contains(equipment)) throw new ArgumentException($"Inventory has no {equipment.Value()}!");

            if (_equipment[equipment].Item2 == _equipment[equipment].Item1) throw new ArgumentException($"{equipment.Value()} is fully equipped!");

            var newEquipment = _equipment.ToDictionary(kvp => kvp.Key, (kvp) => kvp.Value);
            var amount = newEquipment[equipment].Item1;
            var newEquipped = newEquipment[equipment].Item2 + 1;
            newEquipment[equipment] = (amount, newEquipped);
            return new(_id, _items, newEquipment);
        }

        public Inventory Unequip(Equipment equipment)
        {
            if (!_equipment.Keys.Contains(equipment)) throw new ArgumentException($"Inventory has no {equipment.Value()}!");

            if (_equipment[equipment].Item2 < 1) throw new ArgumentException($"No {equipment.Value()} is equipped!");

            var newEquipment = _equipment.ToDictionary(kvp => kvp.Key, (kvp) => kvp.Value);
            var amount = newEquipment[equipment].Item1;
            var newEquipped = newEquipment[equipment].Item2 - 1;
            newEquipment[equipment] = (amount, newEquipped);
            return new(_id, _items, newEquipment);
        }
    }
}