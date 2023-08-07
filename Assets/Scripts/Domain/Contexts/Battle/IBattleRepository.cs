using System;
using System.Collections.Generic;

namespace Battle
{
    public interface IBattleRepository
    {
        public Battle Get(BattleId id);
        public BattleId Create(List<AgentId> players, List<AgentId> enemies, global::BattleFieldId battleFieldId, Phase phase);
        public Battle Update(BattleId battleId, Battle newBattle);
        public IBattleRepository Reload();
        public IBattleRepository Save();
    }
}