using System;

namespace BattleResult
{
    public interface IEnemyRepository
    {
        public Enemy Get(string id);
    }
}