namespace Battle.Services.ActionPrerequisites
{
    public class NotParalyzed : ActionPrerequisite
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