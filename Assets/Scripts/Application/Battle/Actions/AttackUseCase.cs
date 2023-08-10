using System;
using Battle;
using Battle.Services.Actions;

namespace Battle.Actions
{
    public class AttackUseCase
    {
        private UnitOfWork _unitOfWork;

        public AttackUseCase(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionOutcome[] Execute(AgentId actorId, AgentId targetId, BattleId battleId)
        {
            var actor = _unitOfWork.AgentRepository.Get(actorId);
            var target = _unitOfWork.AgentRepository.Get(targetId);
            var battle = _unitOfWork.BattleRepository.Get(battleId);

            return Attack.Execute(actor, target, battle, _unitOfWork);
        }
    }
}