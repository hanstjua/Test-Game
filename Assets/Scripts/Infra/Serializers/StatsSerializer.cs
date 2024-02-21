using System.IO;
using System.Linq;

public class StatsSerializer : ISerializer<Stats>
{
    public Stats Deserialize(byte[] payload)
    {
        using MemoryStream ms = new(payload);
        using BinaryReader br = new(ms);

        return new Stats(
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
        var stats = (Stats) obj;
        using MemoryStream ms = new();
        using BinaryWriter bw = new(ms);

        stats.Value().ToList().ForEach(i => bw.Write(i));

        return ms.ToArray();
    }
}