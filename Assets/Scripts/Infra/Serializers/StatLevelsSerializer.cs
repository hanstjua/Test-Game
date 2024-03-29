using System.IO;
using System.Linq;
using Battle;

public class StatLevelsSerializer : ISerializer<StatLevels>
{
    public StatLevels Deserialize(byte[] payload)
    {
        using MemoryStream ms = new(payload);
        using BinaryReader br = new(ms);

        return new StatLevels(
            br.ReadByte(),
            br.ReadByte(),
            br.ReadByte(),
            br.ReadByte(),
            br.ReadByte(),
            br.ReadByte(),
            br.ReadByte(),
            br.ReadByte(),
            br.ReadByte(),
            br.ReadByte()
        );
    }

    public byte[] Serialize(object obj)
    {
        var stats = (StatLevels) obj;
        using MemoryStream ms = new();
        using BinaryWriter bw = new(ms);

        stats.Value().ToList().ForEach(i => bw.Write(i));

        return ms.ToArray();
    }
}