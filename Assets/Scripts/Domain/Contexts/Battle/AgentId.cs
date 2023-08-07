using System;

[Serializable]
public class AgentId : ValueObject<string>
{
    public AgentId(string uuid)
    {
        Uuid = uuid;
    }

    public string Uuid { get; private set; }
    public override string Value()
    {
        return Uuid;
    }
}
