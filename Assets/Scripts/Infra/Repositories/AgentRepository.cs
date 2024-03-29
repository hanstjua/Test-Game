using System;
using System.Collections.Generic;
using System.Linq;
using Battle;
using Battle.Common;
using Battle.Common.Armours;
using Battle.Common.Weapons;
using Battle.Services.Actions;
using UnityEngine;


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

    public Agent CreateAgentByName(string name, Position position)
    {
        var id = new AgentId(Guid.NewGuid().ToString());
        var agent = new Agent(
            id,
            name,
            new List<Battle.Action> { new Attack(), new Defend() },
            new StatLevels(10, 10, 10, 10, 10, 10, 10, 10, 30, 10),
            position,
            new Dictionary<Item, int>(),
            2,
            new Longsword(),
            new LeatherArmour()
        );

        _agentSet[id] = agent;

        return agent;
    }
    public Agent CreateGenericEnemyByName(string name, Position position)
    {
        var character = CharacterFactory.GetGeneric(name);

        var id = new AgentId(Guid.NewGuid().ToString());
        var agent = new Agent(
            id, 
            name, 
            character.Actions, 
            character.Levels, 
            position, 
            character.Items, 
            character.Movements,
            character.Weapon,
            character.Armour
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