using System;
using System.Collections.Generic;
using Battle;

namespace Battle.Services.Actions
{
    public class AttackService
    {
        public static ActionOutcome[] Execute(Agent actor, Agent target)
        {
            // calculate damage
            double defendMultiplier = 1;
            if (target.HasStatus(Status.Defend))
            {
                defendMultiplier = 0.8;
            }

            int damage = (int)(Math.Ceiling(actor.GetStrength() * 1.2 - target.GetDefense() * 0.8) * defendMultiplier);

            // update target HP
            target.ReduceHp(damage);

            return new ActionOutcome[] {new ActionOutcome((AgentId)target.Id(), hpDamage: damage)};
        }
    }
}
