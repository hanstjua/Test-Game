using System;
using System.Collections.Generic;
using Battle;
using Battle.Armours;
using Battle.Weapons;

public class Serializer
{
    private static readonly Dictionary<Type, ISerializer<object>> _serializers = new() {
        {typeof(Battle.Action), new ActionSerializer()},
        {typeof(Arbellum), new ArbellumSerializer()},
        {typeof(Agent), new AgentSerializer()},
        {typeof(Armour), new ArmourSerializer()},
        {typeof(Position), new PositionSerializer()},
        {typeof(StatLevels), new StatLevelsSerializer()},
        {typeof(Stats), new StatsSerializer()},
        {typeof(Weapon), new WeaponSerializer()},
        {typeof(Inventory.Inventory), new InventorySerializer()}
    };

    public static T Deserialize<T>(byte[] payload) =>  (T) _serializers[typeof(T)].Deserialize(payload);
    public static object Deserialize(Type type, byte[] payload) => _serializers[type].Deserialize(payload);
    public static byte[] Serialize<T>(T t) => _serializers[typeof(T)].Serialize(t);
}