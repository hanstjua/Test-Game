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

        public ActionOutcome[] Execute(string actorId, string targetId)
        {
            var actor = _unitOfWork.AgentRepository.Get(new AgentId(actorId));
            var target = _unitOfWork.AgentRepository.Get(new AgentId(targetId));

            return Attack.Execute(actor, target);
        }
    }
}