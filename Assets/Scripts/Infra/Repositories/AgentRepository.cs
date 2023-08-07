using System;
using System.Collections.Generic;
using System.Linq;
using Battle;
using Battle.Common;
using Battle.Services.Actions;
using UnityEngine;


public class AgentRepository : IAgentRepository
{
    private Dictionary<AgentId, Agent> _agentSet = new Dictionary<AgentId, Agent>();

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

    public Agent CreateAgentByName(string name, Position position)
    {
        var id = new AgentId(Guid.NewGuid().ToString());
        var agent = new Agent(
            id,
            name,
            new List<Battle.Action> { new Attack(), new Defend() },
            new Stats(1, 1, 1, 1, 1, 1, 1, 1),
            10,
            10,
            position,
            new Dictionary<Item, int>(),
            3
        );

        _agentSet[id] = agent;

        return agent;
    }
    public Agent CreateGenericEnemyByName(string name, Position position)
    {
        var character = CharacterFactory.GetGeneric(name);

        Debug.Log($"{position.X} {position.Y} {position.Z}");

        var id = new AgentId(Guid.NewGuid().ToString());
        var agent = new Agent(
            id, 
            name, 
            character.Actions, 
            character.Stats, 
            character.Hp, 
            character.Mp, 
            position, 
            character.Items, 
            character.Movements
        );

        _agentSet[id] = agent;

        return agent;
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