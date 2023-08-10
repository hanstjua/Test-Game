using System;

namespace Battle.Common.Weapons
{
    public interface IWeapon
    {
        public int CalculateDamage(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork);
        public ActionOutcome[] CalculateSideEffects(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork);
    }
}