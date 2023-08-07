using Battle;
using Battle.Services.Actions;

namespace Battle.Actions
{
    public class DefendUseCase
    {
        private UnitOfWork _unitOfWork;

        public DefendUseCase(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionOutcome[] Execute(string actorId)
        {
            var actor = _unitOfWork.AgentRepository.Get(new AgentId(actorId));

            return Defend.Execute(actor);
        }
    }
}