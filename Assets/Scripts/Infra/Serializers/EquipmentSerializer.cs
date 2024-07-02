using System.IO;
using Battle;

public class EquipmentSerializer : ISerializer<Equipment>
{
    public Equipment Deserialize(byte[] payload)
    {
        using MemoryStream ms = new(payload);
        using BinaryReader br = new(ms);

        return EquipmentFactory.Instances[br.ReadString()];
    }

    public byte[] Serialize(object obj)
    {
        var equipment = (Equipment) obj;
        using MemoryStream ms = new();
        using BinaryWriter bw = new(ms);

        bw.Write(equipment.GetType().Name);

        return ms.ToArray();
    }
}