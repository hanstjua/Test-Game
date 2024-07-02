using Common;

namespace Battle.Services.ActionPrerequisites
{
    public class ActorIsAlive : ActionPrerequisite
    {
        public ActorIsAlive() : base("ActorIsAlive")
        {
            
        }

        public override bool IsFulfilledBy(Agent actor, Battle battle, UnitOfWork unitOfWork)
        {
            return actor.IsAlive();
        }
    }
}