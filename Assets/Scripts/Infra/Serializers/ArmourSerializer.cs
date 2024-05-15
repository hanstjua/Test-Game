using System.Collections.Generic;
using System.IO;
using Battle.Common.Armours;

public class ArmourSerializer : ISerializer<Armour>
{
    private static readonly Dictionary<string, Armour> _armourMap = new()
    {
        {ArmourType.LeatherArmour.Name, new LeatherArmour()}
    };

    public Armour Deserialize(byte[] payload)
    {
        using MemoryStream ms = new(payload);
        using BinaryReader br = new(ms);

        return _armourMap[br.ReadString()];
    }

    public byte[] Serialize(object obj)
    {
        var armour = (Armour) obj;
        using MemoryStream ms = new();
        using BinaryWriter bw = new(ms);

        bw.Write(armour.Type.Name);

        return ms.ToArray();
    }
}