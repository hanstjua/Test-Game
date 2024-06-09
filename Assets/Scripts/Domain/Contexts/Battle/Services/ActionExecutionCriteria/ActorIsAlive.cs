using Common;

namespace Battle.Services.ActionExecutionCriteria
{
    public class ActorIsAlive : ActionExecutionCriterion
    {
        public ActorIsAlive() : base("ActorIsAlive")
        {
            
        }

        public override bool IsFulfilledBy(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return actor.IsAlive();
        }
    }
}