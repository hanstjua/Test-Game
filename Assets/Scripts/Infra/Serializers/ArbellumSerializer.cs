using System;
using System.Collections.Generic;
using System.IO;
using Battle;
using Battle.Services.Arbella;

public class ArbellumSerializer : ISerializer<Arbellum>
{
    private static readonly Dictionary<string, Type> _arbellumMap = new()
    {
        {ArbellumType.Physical.Name, typeof(Physical)}
    };

    public Arbellum Deserialize(byte[] payload)
    {
        using MemoryStream ms = new(payload);
        using BinaryReader br = new(ms);

        return (Arbellum) Activator.CreateInstance(_arbellumMap[br.ReadString()], new object[] {br.ReadInt16()});
    }

    public byte[] Serialize(object obj)
    {
        var arbellum = (Arbellum) obj;
        using MemoryStream ms = new();
        using BinaryWriter bw = new(ms);

        bw.Write(arbellum.Type.Name);
        bw.Write(arbellum.Experience);

        return ms.ToArray();
    }
}