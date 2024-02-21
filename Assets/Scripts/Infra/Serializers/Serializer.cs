using System;
using System.Collections.Generic;
using Battle;
using Battle.Common.Armours;
using Battle.Common.Weapons;
using Unity.VisualScripting.YamlDotNet.Serialization;

public class Serializer
{
    private static readonly Dictionary<Type, ISerializer<object>> _serializers = new() {
        {typeof(Battle.Action), new ActionSerializer()},
        {typeof(Agent), new AgentSerializer()},
        {typeof(Armour), new ArmourSerializer()},
        {typeof(Position), new PositionSerializer()},
        {typeof(Stats), new StatsSerializer()},
        {typeof(Weapon), new WeaponSerializer()}
    };

    public static T Deserialize<T>(byte[] payload) =>  (T) _serializers[typeof(T)].Deserialize(payload);

    public static byte[] Serialize<T>(T t) => _serializers[typeof(T)].Serialize(t);
}