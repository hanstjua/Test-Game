using System;
using System.Collections.Generic;
using Battle;

public class BattleRepository : IBattleRepository
{
    private readonly Dictionary<BattleId, Battle.Battle> _battleSet = new();

    public BattleId Create(List<AgentId> players, List<AgentId> enemies, BattleFieldId battleFieldId, Phase phase)
    {
        var id = new BattleId(Guid.NewGuid().ToString());
        _battleSet.Add(id, new Battle.Battle(id, players, enemies, battleFieldId, phase));

        return id;
    }

    public Battle.Battle Get(BattleId id)
    {
        return _battleSet[id];
    }

    public Battle.Battle Update(BattleId id, Battle.Battle newBattle)
    {
        _battleSet[id] = newBattle;

        return newBattle;
    }

    public IBattleRepository Reload()
    {
        return this;
    }

    public IBattleRepository Save()
    {
        return this;
    }
}