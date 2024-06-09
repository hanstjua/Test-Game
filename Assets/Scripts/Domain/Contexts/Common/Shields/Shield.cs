using Common;

namespace Battle.Shields
{
    public abstract class Shield : Handheld
    {
        public override abstract HandheldType Type { get; }
        public abstract int CalculateResistance(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork);
    }
}