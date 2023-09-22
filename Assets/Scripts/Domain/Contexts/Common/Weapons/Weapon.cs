using Battle.Common;

namespace Battle.Common.Weapons
{
    public abstract class Weapon : Equipment
    {
        public abstract WeaponType Type { get; }
        public abstract int CalculateDamage(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork);
    }
}