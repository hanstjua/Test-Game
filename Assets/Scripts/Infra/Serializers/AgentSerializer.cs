using System.IO;
using System.Linq;
using Battle;
using Battle.Accessories;
using Battle.Armours;
using Battle.Footwears;
using Battle.Weapons;

#nullable enable

public class AgentSerializer : ISerializer<Agent>
{
    public Agent Deserialize(byte[] payload)
    {
        using MemoryStream ms = new(payload);
        using BinaryReader br = new(ms);

        var agentId = new AgentId(br.ReadString());
        var name = br.ReadString();
        var arbella = Enumerable.Range(0, br.ReadByte()).Select(_ => Serializer.Deserialize<Arbellum>(br.ReadBytes(br.ReadByte()))).ToArray();
        var statLevels = Serializer.Deserialize<StatLevels>(br.ReadBytes(br.ReadByte()));
        var position = Serializer.Deserialize<Position>(br.ReadBytes(br.ReadByte()));
        var items = Enumerable.Range(0, br.ReadUInt16()).ToDictionary(_ => (Item) br.ReadUInt16(), _ => (int) br.ReadByte());
        var movements = (int) br.ReadByte();
        var rightHand = br.ReadBoolean() ? Serializer.Deserialize<Handheld>(br.ReadBytes(br.ReadByte())) : null;
        var leftHand = br.ReadBoolean() ? Serializer.Deserialize<Handheld>(br.ReadBytes(br.ReadByte())) : null;
        var armour = br.ReadBoolean() ? Serializer.Deserialize<Armour>(br.ReadBytes(br.ReadByte())) : null;
        var footwear = br.ReadBoolean() ? Serializer.Deserialize<Footwear>(br.ReadBytes(br.ReadByte())) : null;
        var accessory1 = br.ReadBoolean() ? Serializer.Deserialize<Accessory>(br.ReadBytes(br.ReadByte())) : null;
        var accessory2 = br.ReadBoolean() ? Serializer.Deserialize<Accessory>(br.ReadBytes(br.ReadByte())) : null;
        var turnGauge = br.ReadDouble();
        var direction = (Direction) br.ReadInt16();

        return new Agent(
            agentId,
            name,
            arbella,
            statLevels,
            position,
            items,
            movements,
            rightHand,
            leftHand,
            armour,
            footwear,
            accessory1,
            accessory2,
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
        bw.Write((byte)agent.Arbella.Count());
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
        
        // write right hand
        bw.Write(agent.RightHand != null);
        if (agent.RightHand != null)
        {
            var rightHandPayload = Serializer.Serialize(agent.RightHand);
            bw.Write((byte) rightHandPayload.Length);
            bw.Write(rightHandPayload);
        }

        // write left hand
        bw.Write(agent.LeftHand != null);
        if (agent.LeftHand != null)
        {
            var leftHandPayload = Serializer.Serialize(agent.LeftHand);
            bw.Write((byte) leftHandPayload.Length);
            bw.Write(leftHandPayload);
        }

        // write armour
        bw.Write(agent.Armour != null);
        if (agent.Armour != null)
        {
            var armourPayload = Serializer.Serialize(agent.Armour);
            bw.Write((byte) armourPayload.Length);
            bw.Write(armourPayload);
        }

        // write footwear
        bw.Write(agent.Footwear != null);
        if (agent.Footwear != null)
        {
            var footwearPayload = Serializer.Serialize(agent.Footwear);
            bw.Write((byte) footwearPayload.Length);
            bw.Write(footwearPayload);
        }

        // write accessory 1
        bw.Write(agent.Accessory1 != null);
        if (agent.Accessory1 != null)
        {
            var accessoryPayload = Serializer.Serialize(agent.Accessory1);
            bw.Write((byte) accessoryPayload.Length);
            bw.Write(accessoryPayload);
        }

        // write footwear
        bw.Write(agent.Accessory2 != null);
        if (agent.Accessory1 != null)
        {
            var accessoryPayload = Serializer.Serialize(agent.Accessory2);
            bw.Write((byte) accessoryPayload.Length);
            bw.Write(accessoryPayload);
        }

        bw.Write(agent.TurnGauge);
        bw.Write((short) agent.Direction);

        return ms.ToArray();
    }
}

#nullable disable