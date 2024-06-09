using Common;

namespace Battle.Weapons
{
    public abstract class Weapon : Handheld
    {
        public override abstract HandheldType Type { get; }
        public abstract int CalculateDamage(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork);
    }
}