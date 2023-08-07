using System;
using Battle.Common;
using Battle;

namespace Battle.Services.Actions
{
    public class UseItem : Action
    {
        public override Position[] AreaOfEffect => throw new NotImplementedException();
        public override string Name => "Item";
        public static ActionOutcome[] Execute(Agent actor, Agent target, Item item)
        {
            ActionOutcome ret;

            switch (item)
            {
                case Item.Potion:
                    if (!target.IsAlive())
                    {
                        throw new InvalidOperationException("Target is KO'd.");
                    }

                    actor.ConsumeItem(item);
                    target.RestoreHp(300);
                    ret = new ActionOutcome((AgentId)target.Id(), hpDamage: -300);
                    break;

                case Item.Ether:
                    if (!target.IsAlive())
                    {
                        throw new InvalidOperationException("Target is KO'd.");
                    }

                    actor.ConsumeItem(item);
                    target.RestoreMp(50);
                    ret = new ActionOutcome((AgentId)target.Id(), mpDamage: -50);
                    break;

                case Item.Grenade:
                    if (!target.IsAlive())
                    {
                        throw new InvalidOperationException("Target is KO'd.");
                    }

                    actor.ConsumeItem(item);
                    target.ReduceHp(100);
                    ret = new ActionOutcome((AgentId)target.Id(), hpDamage: 100);
                    break;

                default:
                    throw new InvalidOperationException(String.Format("Unhandled item {0}!", item));
            }

            return new ActionOutcome[] {ret};
        }
    }
}