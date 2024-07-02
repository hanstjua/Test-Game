using Common;

namespace Battle.Shields
{
    public abstract class Shield : Handheld
    {
        public abstract int CalculateResistance(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork);
    }
}