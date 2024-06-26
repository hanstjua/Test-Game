using Battle;
using System;
using System.IO;

public class ItemSerializer : ISerializer<Item>
{
    public Item Deserialize(byte[] payload)
    {
        using MemoryStream ms = new(payload);
        using BinaryReader br = new(ms);

        return ItemFactory.Instances[br.ReadString()];
    }

    public byte[] Serialize(object obj)
    {
        var item = (Item) obj;
        using MemoryStream ms = new();
        using BinaryWriter bw = new(ms);

        bw.Write(item.Name);

        return ms.ToArray();
    }
}