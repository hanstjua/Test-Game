using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Statuses;

namespace Battle.Services.Actions
{
    public class Attack : Action
    {
        public Attack() : base("Attack")
        {
        }

        public override AreaOfEffect AreaOfEffect => new AreaOfEffect(
            new Position[] {
                new Position(1, 1, 0),
                new Position(1, 0, 0),
                new Position(1, -1, 0),
                new Position(0, 1, 0),
                new Position(0, -1, 0),
                new Position(-1, 1, 0),
                new Position(-1, 0, 0),
                new Position(-1, -1, 0)
            },
            2
        );

        public override ActionType Type => ActionType.Attack;

        protected override ActionOutcome OnExecute(Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            // calculate damage
            var target = targets[0];
            int weaponDamage = actor.Weapon.CalculateDamage(actor, target, battle, unitOfWork);
            int armourResistance = target.Armour.CalculateResistance(actor, target, battle, unitOfWork);

            // TODO: calculate damage enhancement by accessories
            // TODO: calculate damage attenuation by accessories

            var damage = weaponDamage - armourResistance;

            var actionEffects = new ActionEffect[] {new HpDamage(target.Id() as AgentId, damage)};

            if (damage > target.Hp)  // target is KO'd
            {
                actionEffects = actionEffects.Append(new AddStatus(target.Id() as AgentId, new KO(), battle.Id() as BattleId)).ToArray();
            }

            return new ActionOutcome(
                actor.Id() as AgentId,
                new AgentId[] {target.Id() as AgentId},
                Type,
                actionEffects
            );
        }

        protected override bool ShouldExecute(Agent target, Agent actor)
        {
            return target.IsAlive() && actor.IsAlive();
        }
    }
}
