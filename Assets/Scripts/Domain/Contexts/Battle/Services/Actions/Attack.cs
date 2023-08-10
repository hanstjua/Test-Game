using System;
using System.Collections.Generic;
using Battle;

namespace Battle.Services.Actions
{
    public class Attack : Action
    {
        public override Position[] AreaOfEffect => throw new NotImplementedException();  // get attack AOE from weapon

        public override string Name => "Attack";
        public static ActionOutcome[] Execute(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            // calculate damage
            double defendMultiplier = 1;
            if (target.HasStatus(Status.Defend))
            {
                defendMultiplier = 0.8;
            }

            int weaponDamage = actor.Weapon.CalculateDamage(actor, target, battle, unitOfWork);
            // TODO: calculate damage enhancement by armor/accessories
            // TODO: calculate damage attenuation by armor/accessories
            int damage = (int)(Math.Ceiling(weaponDamage - target.GetDefense() * 0.8) * defendMultiplier);

            // TODO: handle weapon/armor/accessories side effects

            return new ActionOutcome[] {new ActionOutcome((AgentId)target.Id(), hpDamage: damage)};
        }
    }
}
