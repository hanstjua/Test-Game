using System;
using System.Linq;
using Battle.Common;
using Battle;

namespace Battle.Services.Actions
{
    public class UseItem : Action
    {
        public override AreaOfEffect AreaOfEffect => throw new NotImplementedException();
        public override string Name => "Item";
        public override ActionType Type => ActionType.UseItem;

        public ActionOutcome[] PreExecute(Agent actor, Agent target, Item item, Battle battle, UnitOfWork unitOfWork)
        {
            ActionOutcome[] ret = {};

            var actorWeaponEffects = actor.Weapon.ApplyPreExecutionInitiatorEffects(ret, actor, target, battle, unitOfWork);
            var actorArmourEffects = actor.Armour.ApplyPreExecutionInitiatorEffects(ret, actor, target, battle, unitOfWork);

            var targetWeaponEffects = target.Weapon.ApplyPreExecutionReactorEffects(ret, actor, target, battle, unitOfWork);
            var targetArmourEffects = target.Armour.ApplyPreExecutionReactorEffects(ret, actor, target, battle, unitOfWork);

            return actorWeaponEffects
            .Concat(actorArmourEffects)
            .Concat(targetWeaponEffects)
            .Concat(targetArmourEffects)
            .ToArray();
        }

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

        public ActionOutcome[] PostExecute(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            ActionOutcome[] ret = {};

            var actorWeaponEffects = actor.Weapon.ApplyPostExecutionInitiatorEffects(ret, actor, target, battle, unitOfWork);
            var actorArmourEffects = actor.Armour.ApplyPostExecutionInitiatorEffects(ret, actor, target, battle, unitOfWork);

            var targetWeaponEffects = target.Weapon.ApplyPostExecutionReactorEffects(ret, actor, target, battle, unitOfWork);
            var targetArmourEffects = target.Armour.ApplyPostExecutionReactorEffects(ret, actor, target, battle, unitOfWork);

            return actorWeaponEffects
            .Concat(actorArmourEffects)
            .Concat(targetWeaponEffects)
            .Concat(targetArmourEffects)
            .ToArray();
        }
    }
}