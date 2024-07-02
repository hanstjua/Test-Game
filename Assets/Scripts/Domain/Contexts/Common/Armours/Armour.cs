using Common;

namespace Battle.Armours
{
    public abstract class Armour : Equipment
    {
        public abstract int CalculateResistance(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork);
    }
}