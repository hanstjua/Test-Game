using System;
using System.Linq;
using Battle.Common;

namespace Battle
{
    public class PreemptorService
    {
        public ActionOutcome[] Execute(Agent preemptor, Agent[] potentialPreemptors, ActionType action, Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {

            var role = Equipment.GetHolderRole(preemptor.Id() as AgentId, actor.Id() as AgentId, targets.Select(a => a.Id() as AgentId).ToArray());

            Func<bool> isWeaponTriggered = () => preemptor.Weapon.IsPreExecutionEffectsTriggered(role, action, preemptor, actor, targets, battle, unitOfWork);
            Func<bool> isArmourTriggered = () => preemptor.Armour.IsPreExecutionEffectsTriggered(role, action, preemptor, actor, targets, battle, unitOfWork);

            ActionOutcome[] ret = {};

            if (!isWeaponTriggered() && !isArmourTriggered())
            {
                return ret;
            }

            var preemptors = potentialPreemptors.Where(a => !a.Id().Equals(preemptor.Id())).ToArray();

            if (isWeaponTriggered())
            {
                var outcome = preemptor.Weapon.GetPreExecutionEffects(role, action, preemptor, actor, targets, battle, unitOfWork);
                var effectTargets = outcome.On.Select(i => unitOfWork.AgentRepository.Get(i)).ToArray();
                var weaponOutcomes = preemptors
                .Aggregate(new ActionOutcome[] {}, 
                (outcomes, preemptor) => outcomes.Concat(Execute(preemptor, preemptors, outcome.Cause, preemptor, effectTargets, battle, unitOfWork)).ToArray())
                .ToArray();

                if (isWeaponTriggered())
                {
                    ApplyActionOutcomeService.Execute(outcome, unitOfWork);
                    weaponOutcomes = weaponOutcomes.Append(outcome).ToArray();
                }
                
                ret = ret.Concat(weaponOutcomes).ToArray();
            }

            if (isArmourTriggered())
            {
                var outcome = preemptor.Armour.GetPreExecutionEffects(role, action, preemptor, actor, targets, battle, unitOfWork);
                var effectTargets = outcome.On.Select(i => unitOfWork.AgentRepository.Get(i)).ToArray();
                var armourOutcomes = preemptors
                .Aggregate(new ActionOutcome[] {}, 
                (outcomes, preemptor) => outcomes.Concat(Execute(preemptor, preemptors, outcome.Cause, preemptor, effectTargets, battle, unitOfWork)).ToArray())
                .ToArray();

                if (isArmourTriggered())
                {
                    ApplyActionOutcomeService.Execute(outcome, unitOfWork);
                    armourOutcomes = armourOutcomes.Append(outcome).ToArray();
                }

                ret = ret.Concat(armourOutcomes).ToArray();
            }

            return ret;
        }
    }
}