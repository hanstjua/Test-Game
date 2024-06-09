using System;
using System.Collections.Generic;
using System.Linq;
using Battle;

public class AgentRepository : IAgentRepository
{
    private readonly Dictionary<AgentId, Agent> _agentSet = new();

    public Agent Get(AgentId id)
    {
        return _agentSet[id];
    }

    public Agent[] GetAll()
    {
        return _agentSet.Values.ToArray();
    }

    public Agent GetFirstBy(Predicate<Agent> predicate)
    {
        return _agentSet.Values.ToList().Find(predicate);
    }

    public bool Remove(AgentId id)
    {
        return _agentSet.Remove(id);
    }

    public Agent Update(AgentId id, Agent newAgent)
    {
        _agentSet[id] = newAgent;

        return newAgent;
    }

    public IAgentRepository Reload()
    {
        return this;
    }

    public IAgentRepository Save() 
    {
        return this;
    }
}