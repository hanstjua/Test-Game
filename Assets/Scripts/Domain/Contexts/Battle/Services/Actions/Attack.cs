using System;
using System.Linq;
using Battle;
using Battle.Services.ActionPrerequisites;
using Battle.Statuses;

namespace Battle.Services.Actions
{
    public class Attack : Action
    {
        public Attack() : base("Attack", "Use weapon(s) in hand to inflict physical damage.")
        {
        }

        public override AreaOfEffect AreaOfEffect => new(
            new Position[] {new(0,0,0)},
            1
        );

        public override AreaOfEffect TargetArea => new(
            new Position[] {
                new(1, 1, 0),
                new(1, 0, 0),
                new(1, -1, 0),
                new(0, 1, 0),
                new(0, -1, 0),
                new(-1, 1, 0),
                new(-1, 0, 0),
                new(-1, -1, 0)
            },
            2
        );

        public override ActionType Type => ActionType.Attack;

        public override SkillType Skill => SkillType.Physical;

        public override ActionPrerequisite[] Criteria => new[] { new NotParalyzed() };

        public override StatType[] ActorRelevantStats => new StatType[] { StatType.Strength };

        public override StatType[] TargetRelevantStats => new StatType[] { StatType.Defense };

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

        public override bool CanExecute(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            return true;
        }

        protected override bool ShouldExecute(Agent target, Agent actor)
        {
            return target.IsAlive() && actor.IsAlive();
        }
    }
}
