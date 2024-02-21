using System.Collections.Generic;
using System.IO;
using System.Linq;
using Battle;
using Battle.Services.Actions;

public class ActionSerializer : ISerializer<Action>
{
    private static readonly Dictionary<string, Action> _actionMap = new()
    {
        {new Attack().Name, new Attack()},
        {new Defend().Name, new Defend()},
        {new Earth().Name, new Earth()},
        {new Fire().Name, new Fire()},
        {new Ice().Name, new Ice()},
        {new Thunder().Name, new Thunder()},
        {new UseItem().Name, new UseItem()},
        {new Water().Name, new Water()},
        {new Wind().Name, new Wind()},
    };

    public Action Deserialize(byte[] payload)
    {
        using MemoryStream ms = new(payload);
        using BinaryReader br = new(ms);

        var name = br.ReadString();

        return _actionMap[name];
    }

    public byte[] Serialize(object obj)
    {
        var action = (Action) obj;
        using MemoryStream ms = new();
        using BinaryWriter bw = new(ms);

        bw.Write(_actionMap[action.Name].Name);

        return ms.ToArray();
    }
}