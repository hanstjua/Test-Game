using System;
using System.Linq;
using Battle.Common;
using Battle;
using Battle.Services.ActionCriteria;

namespace Battle.Services.Actions
{
    public class UseItem : Action
    {
        public UseItem() : base("Item", "Use item.")
        {
        }

        public override AreaOfEffect AreaOfEffect => throw new NotImplementedException();
        public override ActionType Type => ActionType.UseItem;

        public override SkillType Skill => SkillType.Item;
        public override ActionCriterion[] Criteria => new[] { new NotParalyzed() };

        public ActionEffect[] Execute(Agent actor, Agent target, Item item)
        {
            ActionEffect effectOnTarget;

            switch (item)
            {
                case Item.Potion:
                    if (!target.IsAlive())
                    {
                        throw new InvalidOperationException("Target is KO'd.");
                    }

                    actor.ConsumeItem(item);
                    target.RestoreHp(300);
                    effectOnTarget = new HpDamage(target.Id() as AgentId, -300);
                    break;

                case Item.Ether:
                    if (!target.IsAlive())
                    {
                        throw new InvalidOperationException("Target is KO'd.");
                    }

                    actor.ConsumeItem(item);
                    target.RestoreMp(50);
                    effectOnTarget = new MpDamage((AgentId)target.Id(), -50);
                    break;

                case Item.Grenade:
                    if (!target.IsAlive())
                    {
                        throw new InvalidOperationException("Target is KO'd.");
                    }

                    actor.ConsumeItem(item);
                    target.ReduceHp(100);
                    effectOnTarget = new HpDamage((AgentId)target.Id(), 100);
                    break;

                default:
                    throw new InvalidOperationException(String.Format("Unhandled item {0}!", item));
            }

            return new ActionEffect[] { new ItemLost(actor.Id() as AgentId, item, 1), effectOnTarget };
        }

        protected override ActionOutcome OnExecute(Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            throw new NotImplementedException();
        }

        protected override bool ShouldExecute(Agent target, Agent actor)
        {
            throw new NotImplementedException();
        }
    }
}