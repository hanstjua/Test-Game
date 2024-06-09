using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Battle;
using Battle.Accessories;
using Battle.Armours;
using Battle.Footwears;
using Battle.Shields;
using Battle.Weapons;
using Inventory;

public class InventorySerializer : ISerializer<Inventory.Inventory>
{
    private static readonly Dictionary<Type, byte> _typeToIndicator = new() {
        {typeof(Weapon), 0},
        {typeof(Shield), 1},
        {typeof(Armour), 2},
        {typeof(Footwear), 3},
        {typeof(Accessory), 4},
    };

    private static readonly Dictionary<byte, Type> _indicatorToType = _typeToIndicator.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

    public Inventory.Inventory Deserialize(byte[] payload)
    {
        using MemoryStream ms = new(payload);
        using BinaryReader br = new(ms);

        var id = br.ReadString();

        Dictionary<Item, int> items = new();
        var itemCount = br.ReadInt32();
        for (int i = 0; i < itemCount; i++)
        {
            items.Add(Serializer.Deserialize<Item>(br.ReadBytes(br.ReadByte())), br.ReadByte());
        }

        Dictionary<Equipment, (int, int)> equipment = new();
        var equipmentCount = br.ReadInt32();
        for (int i = 0; i < equipmentCount; i++)
        {
            var type = _indicatorToType[br.ReadByte()];
            equipment.Add((Equipment) Serializer.Deserialize(type, br.ReadBytes(br.ReadByte())), (br.ReadByte(), br.ReadByte()));
        }

        return new(new InventoryId(id), items, equipment);
    }

    public byte[] Serialize(object obj)
    {
        var inventory = (Inventory.Inventory) obj;
        using MemoryStream ms = new();
        using BinaryWriter bw = new(ms);

        // serialize id
        bw.Write(((InventoryId) inventory.Id()).Value());

        // serialize items
        bw.Write(inventory.AllItems.Count());
        foreach (var item in inventory.AllItems)
        {
            var payload = Serializer.Serialize(item);
            bw.Write((byte) payload.Length);
            bw.Write(payload);
            bw.Write(inventory.Amount(item));
        }

        // serialize equipment
        bw.Write(inventory.AllEquipment.Count());
        foreach (var equipment in inventory.AllEquipment)
        {
            if (equipment is Weapon weapon)
            {
                bw.Write(_typeToIndicator[typeof(Weapon)]);
                var payload = Serializer.Serialize(weapon);
                bw.Write((byte) payload.Length);
                bw.Write(payload);
                bw.Write((byte) inventory.Amount(weapon));
                bw.Write((byte) inventory.Equipped(weapon));
            }
            else if (equipment is Shield shield)
            {
                bw.Write(_typeToIndicator[typeof(Shield)]);
                var payload = Serializer.Serialize(shield);
                bw.Write((byte) payload.Length);
                bw.Write(payload);
                bw.Write((byte) inventory.Amount(shield));
                bw.Write((byte) inventory.Equipped(shield));
            }
            else if (equipment is Armour armour)
            {
                bw.Write(_typeToIndicator[typeof(Armour)]);
                var payload = Serializer.Serialize(armour);
                bw.Write((byte) payload.Length);
                bw.Write(payload);
                bw.Write((byte) inventory.Amount(armour));
                bw.Write((byte) inventory.Equipped(armour));
            }
            else if (equipment is Footwear footwear)
            {
                bw.Write(_typeToIndicator[typeof(Footwear)]);
                var payload = Serializer.Serialize(footwear);
                bw.Write((byte) payload.Length);
                bw.Write(payload);
                bw.Write((byte) inventory.Amount(footwear));
                bw.Write((byte) inventory.Equipped(footwear));
            }
            else if (equipment is Accessory accessory)
            {
                bw.Write(_typeToIndicator[typeof(Accessory)]);
                var payload = Serializer.Serialize(accessory);
                bw.Write((byte) payload.Length);
                bw.Write(payload);
                bw.Write((byte) inventory.Amount(accessory));
                bw.Write((byte) inventory.Equipped(accessory));
            }
        }

        return ms.ToArray();
    }
}