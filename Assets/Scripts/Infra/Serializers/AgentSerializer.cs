using System.IO;
using System.Linq;
using Battle;
using Battle.Common;
using Battle.Common.Armours;
using Battle.Common.Weapons;

public class AgentSerializer : ISerializer<Agent>
{
    public Agent Deserialize(byte[] payload)
    {
        using MemoryStream ms = new(payload);
        using BinaryReader br = new(ms);

        var agentId = new AgentId(br.ReadString());
        var name = br.ReadString();
        var actions = Enumerable.Range(0, br.ReadByte()).Select(_ => Serializer.Deserialize<Action>(br.ReadBytes(br.ReadByte()))).ToList();
        var statLevels = Serializer.Deserialize<StatLevels>(br.ReadBytes(br.ReadByte()));
        var position = Serializer.Deserialize<Position>(br.ReadBytes(br.ReadByte()));
        var items = Enumerable.Range(0, br.ReadUInt16()).ToDictionary(_ => (Item) br.ReadUInt16(), _ => (int) br.ReadByte());
        var movements = (int) br.ReadByte();
        var weapon = Serializer.Deserialize<Weapon>(br.ReadBytes(br.ReadByte()));
        var armour = Serializer.Deserialize<Armour>(br.ReadBytes(br.ReadByte()));
        var turnGauge = br.ReadDouble();
        var direction = (Direction) br.ReadInt16();

        return new Agent(
            agentId,
            name,
            actions,
            statLevels,
            position,
            items,
            movements,
            weapon,
            armour,
            turnGauge,
            direction
        );
    }

    public byte[] Serialize(object obj)
    {
        var agent = (Agent) obj;
        using MemoryStream ms = new();
        using BinaryWriter bw = new(ms);

        bw.Write(((AgentId) agent.Id()).Uuid);
        bw.Write(agent.Name);

        // write actions
        bw.Write((byte)agent.Actions.Count);
        foreach (var action in agent.Actions)
        {
            var actionPayload = Serializer.Serialize(action);
            bw.Write((byte) actionPayload.Length);
            bw.Write(actionPayload);
        }

        // write stats
        var statsPayload = Serializer.Serialize(agent.Stats);
        bw.Write((byte) statsPayload.Length);
        bw.Write(statsPayload);

        // write position
        var positionPayload = Serializer.Serialize(agent.Position);
        bw.Write((byte) positionPayload.Length);
        bw.Write(positionPayload);

        // write items
        bw.Write((ushort) agent.Items.Count);
        foreach (var kvp in agent.Items)
        {
            bw.Write((ushort) kvp.Key);
            bw.Write((byte) kvp.Value);
        }

        bw.Write((byte) agent.Movements);
        
        // write weapon
        var weaponPayload = Serializer.Serialize(agent.Weapon);
        bw.Write((byte) weaponPayload.Length);
        bw.Write(weaponPayload);

        // write armour
        var armourPayload = Serializer.Serialize(agent.Armour);
        bw.Write((byte) armourPayload.Length);
        bw.Write(armourPayload);

        bw.Write(agent.TurnGauge);
        bw.Write((short) agent.Direction);

        return ms.ToArray();
    }
}