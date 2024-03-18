namespace Battle.Services.ActionExecutionCriteria
{
    public class TargetIsAlive : ActionExecutionCriterion
    {
        public TargetIsAlive() : base("TargetIsAlive")
        {
            
        }

        public override bool IsFulfilledBy(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return target.IsAlive();
        }
    }
}