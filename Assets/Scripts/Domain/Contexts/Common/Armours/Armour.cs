using Battle.Common;

namespace Battle.Common.Armours
{
    public abstract class Armour : Equipment
    {
        public abstract ArmourType Type { get; }
        public abstract int CalculateResistance(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork);
    }
}