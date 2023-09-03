using System;
using System.Linq;

namespace Battle
{
    public class Defeat : Phase
    {
        public UnitOfWork unitOfWork { get; private set; }
        public Defeat(UnitOfWork unitOfWork) : base("Battle.Defeat") { this.unitOfWork = unitOfWork; }
        public override Phase Transition(Battle battle)
        {
            using (unitOfWork)
            {
                battle.EnemyIds.Concat(battle.PlayerIds).Select(id => unitOfWork.AgentRepository.Remove(id));
                unitOfWork.Save();
            }

            return this;
        }
    }
}