using System;

[Serializable]
public class BattleFieldId : ValueObject<string>
{
    public BattleFieldId(string uuid)
    {
        Uuid = uuid;
    }

    public override string Value()
    {
        return Uuid;
    }

    public string Uuid { get; private set; }
}