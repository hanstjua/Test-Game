using System.Collections.Generic;
using System.IO;
using System.Linq;
using Battle;
using Battle.Services.Actions;

public class ActionSerializer : ISerializer<Action>
{
    private static readonly Dictionary<string, Action> _actionMap = new()
    {
        {new Attack().Type.Name, new Attack()},
        {new Defend().Type.Name, new Defend()},
        {new Earth().Type.Name, new Earth()},
        {new Fire().Type.Name, new Fire()},
        {new Ice().Type.Name, new Ice()},
        {new Thunder().Type.Name, new Thunder()},
        {new UseItem().Type.Name, new UseItem()},
        {new Water().Type.Name, new Water()},
        {new Wind().Type.Name, new Wind()},
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

        bw.Write(_actionMap[action.Type.Name].Type.Name);

        return ms.ToArray();
    }
}