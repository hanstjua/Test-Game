using Battle.Common;
using Battle.Services.Actions;

namespace Battle.Actions
{
    public class UseItemUseCase
    {
        private UnitOfWork _unitOfWork;

        public UseItemUseCase(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionOutcome[] Execute(string actorId, string targetId, Item item)
        {
            var actor = _unitOfWork.AgentRepository.Get(new AgentId(actorId));
            var target = _unitOfWork.AgentRepository.Get(new AgentId(targetId));

            return UseItem.Execute(actor, target, item);
        }
    }
}