using System;
using System.Collections.Generic;
using System.Linq;

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

        // public ActionOutcome[] PreExecute(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        // {
        //     ActionOutcome[] ret = {};

        //     var actorWeaponEffects = actor.Weapon.ApplyPreExecutionInitiatorEffects(ret, actor, target, battle, unitOfWork);
        //     var actorArmourEffects = actor.Armour.ApplyPreExecutionInitiatorEffects(ret, actor, target, battle, unitOfWork);

        //     var targetWeaponEffects = target.Weapon.ApplyPreExecutionReactorEffects(ret, actor, target, battle, unitOfWork);
        //     var targetArmourEffects = target.Armour.ApplyPreExecutionReactorEffects(ret, actor, target, battle, unitOfWork);

        //     return actorWeaponEffects
        //     .Concat(actorArmourEffects)
        //     .Concat(targetWeaponEffects)
        //     .Concat(targetArmourEffects)
        //     .ToArray();
        // }

        // public ActionOutcome Execute(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        // {
        //     if (actor.IsAlive() && target.IsAlive())  // make sure actor and target are still alive
        //     {
        //         // calculate damage
        //         int weaponDamage = actor.Weapon.CalculateDamage(actor, target, battle, unitOfWork);
        //         int armourResistance = target.Armour.CalculateResistance(actor, target, battle, unitOfWork);

        //         // TODO: calculate damage enhancement by accessories
        //         // TODO: calculate damage attenuation by accessories

        //         var damage = weaponDamage - armourResistance;

        //         return new ActionOutcome(
        //             actor.Id() as AgentId,
        //             new AgentId[] {target.Id() as AgentId},
        //             Type,
        //             new ActionEffect[] {new HpDamage(target.Id() as AgentId, damage)}
        //         );
        //     }
        //     else return null;
        // }

        protected override ActionOutcome OnExecute(Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            // calculate damage
            var target = targets[0];
            int weaponDamage = actor.Weapon.CalculateDamage(actor, target, battle, unitOfWork);
            int armourResistance = target.Armour.CalculateResistance(actor, target, battle, unitOfWork);

            // TODO: calculate damage enhancement by accessories
            // TODO: calculate damage attenuation by accessories

            var damage = weaponDamage - armourResistance;

            return new ActionOutcome(
                actor.Id() as AgentId,
                new AgentId[] {target.Id() as AgentId},
                Type,
                new ActionEffect[] {new HpDamage(target.Id() as AgentId, damage)}
            );
        }

        // public ActionOutcome[] PostExecute(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        // {
        //     ActionOutcome[] ret = {};

        //     var actorWeaponEffects = actor.Weapon.ApplyPostExecutionInitiatorEffects(ret, actor, target, battle, unitOfWork);
        //     var actorArmourEffects = actor.Armour.ApplyPostExecutionInitiatorEffects(ret, actor, target, battle, unitOfWork);

        //     var targetWeaponEffects = target.Weapon.ApplyPostExecutionReactorEffects(ret, actor, target, battle, unitOfWork);
        //     var targetArmourEffects = target.Armour.ApplyPostExecutionReactorEffects(ret, actor, target, battle, unitOfWork);

        //     return actorWeaponEffects
        //     .Concat(actorArmourEffects)
        //     .Concat(targetWeaponEffects)
        //     .Concat(targetArmourEffects)
        //     .ToArray();
        // }

        protected override bool ShouldExecute(Agent target, Agent actor)
        {
            return target.IsAlive() && actor.IsAlive();
        }
    }
}
