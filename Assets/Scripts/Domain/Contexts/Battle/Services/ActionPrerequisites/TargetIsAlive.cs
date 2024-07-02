using Common;

namespace Battle.Services.ActionPrerequisites
{
    public class TargetIsAlive : ActionPrerequisite
    {
        public TargetIsAlive() : base("TargetIsAlive")
        {
            
        }

        public override bool IsFulfilledBy(Agent actor, Battle battle, UnitOfWork unitOfWork)
        {
            return true;
        }
    }
}