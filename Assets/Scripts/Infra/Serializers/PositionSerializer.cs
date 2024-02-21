using System.IO;
using Battle;

public class PositionSerializer : ISerializer<Position>
{
    public Position Deserialize(byte[] payload)
    {
        using MemoryStream ms = new(payload);
        using BinaryReader br = new(ms);

        return new Position(br.ReadByte(), br.ReadByte(), br.ReadByte());
    }

    public byte[] Serialize(object obj)
    {
        var position = (Position) obj;
        using MemoryStream ms = new();
        using BinaryWriter bw = new(ms);

        bw.Write((byte)position.X);
        bw.Write((byte)position.Y);
        bw.Write((byte)position.Z);

        return ms.ToArray();
    }
}