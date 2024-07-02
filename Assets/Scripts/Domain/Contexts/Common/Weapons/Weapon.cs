using Common;

namespace Battle.Weapons
{
    public abstract class Weapon : Handheld
    {
        public abstract int CalculateDamage(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork);
    }
}