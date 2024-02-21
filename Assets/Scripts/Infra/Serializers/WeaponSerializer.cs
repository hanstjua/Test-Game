using System.Collections.Generic;
using System.IO;
using Battle.Common.Weapons;

public class WeaponSerializer : ISerializer<Weapon>
{
    private static readonly Dictionary<string, Weapon> _weaponMap = new()
    {
        {WeaponType.Longsword.Name, new Longsword()}
    };

    public Weapon Deserialize(byte[] payload)
    {
        using MemoryStream ms = new(payload);
        using BinaryReader br = new(ms);

        return _weaponMap[br.ReadString()];
    }

    public byte[] Serialize(object obj)
    {
        var weapon = (Weapon) obj;
        using MemoryStream ms = new();
        using BinaryWriter bw = new(ms);

        bw.Write(_weaponMap[weapon.Type.Name].Type.Name);

        return ms.ToArray();
    }
}