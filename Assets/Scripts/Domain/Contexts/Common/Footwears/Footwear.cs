using Common;

namespace Battle.Footwears
{
    public abstract class Footwear : Equipment
    {
        public abstract int CalculateResistance(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork);
    }
}