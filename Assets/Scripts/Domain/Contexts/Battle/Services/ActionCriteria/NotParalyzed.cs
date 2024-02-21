using System;

namespace Battle.Services.ActionCriteria
{
    public class NotParalyzed : ActionCriterion
    {
        public NotParalyzed() : base("NotParalyzed")
        {
            
        }

        public override bool IsFulfilledBy(Agent actor, Battle battle, UnitOfWork unitOfWork)
        {
            return true;
        }
    }
}