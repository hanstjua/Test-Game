using System;

namespace Battle.Common.Weapons
{
    public class Longsword : IWeapon
    {
        public int CalculateDamage(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return 8;
        }

        public ActionOutcome[] CalculateSideEffects(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return new ActionOutcome[] {};
        }
    }
}