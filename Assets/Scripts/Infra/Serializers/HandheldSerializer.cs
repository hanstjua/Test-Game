using System.Collections.Generic;
using System.IO;
using Battle;
using Battle.Shieds;
using Battle.Shields;
using Battle.Weapons;

public class HandheldSerializer : ISerializer<Handheld>
{
    private static readonly Dictionary<string, Handheld> _handheldMap = new()
    {
        // Weapons
        {WeaponType.Longsword.Name, new Longsword()},

        // Shields
        {ShieldType.Buckler.Name, new Buckler()}
    };

    public Handheld Deserialize(byte[] payload)
    {
        using MemoryStream ms = new(payload);
        using BinaryReader br = new(ms);

        return _handheldMap[br.ReadString()];
    }

    public byte[] Serialize(object obj)
    {
        var handheld = (Handheld) obj;
        using MemoryStream ms = new();
        using BinaryWriter bw = new(ms);

        bw.Write(_handheldMap[handheld.Type.Name].Type.Name);

        return ms.ToArray();
    }
}