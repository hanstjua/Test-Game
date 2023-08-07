using System;
using System.Linq;

namespace Battle
{
    public class Victory : Phase
    {
        public UnitOfWork unitOfWork { get; private set; }
        public Victory(UnitOfWork unitOfWork) : base("Battle.Victory") { this.unitOfWork = unitOfWork; }
        public override Phase Transition(Battle battle)
        {
            battle.EnemyIds.Concat(battle.PlayerIds).Select(id => unitOfWork.AgentRepository.Remove(id));
            unitOfWork.Save();
            
            return this;
        }
    }
}