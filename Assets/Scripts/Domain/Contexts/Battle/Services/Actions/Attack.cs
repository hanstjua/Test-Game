using System;
using System.Collections.Generic;
using Battle;

namespace Battle.Services.Actions
{
    public class Attack : Action
    {
        public override Position[] AreaOfEffect => throw new NotImplementedException();

        public override string Name => "Attack";
        public static ActionOutcome[] Execute(Agent actor, Agent target)
        {
            // calculate damage
            double defendMultiplier = 1;
            if (target.HasStatus(Status.Defend))
            {
                defendMultiplier = 0.8;
            }

            int damage = (int)(Math.Ceiling(actor.GetStrength() * 1.2 - target.GetDefense() * 0.8) * defendMultiplier);

            return new ActionOutcome[] {new ActionOutcome((AgentId)target.Id(), hpDamage: damage)};
        }
    }
}
