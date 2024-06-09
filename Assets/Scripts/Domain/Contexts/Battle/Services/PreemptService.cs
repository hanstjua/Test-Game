using Common;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public class PreemptorService
    {
        public ActionOutcome[] Execute(Agent preemptor, Agent[] potentialPreemptors, ActionType action, Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {

            var role = Equipment.GetHolderRole(preemptor.Id() as AgentId, actor.Id() as AgentId, targets.Select(a => a.Id() as AgentId).ToArray());

            bool isWeaponTriggered() => preemptor.RightHand.IsPreExecutionEffectsTriggered(role, action, preemptor, actor, targets, battle, unitOfWork);
            bool isArmourTriggered() => preemptor.Armour.IsPreExecutionEffectsTriggered(role, action, preemptor, actor, targets, battle, unitOfWork);

            if (!isWeaponTriggered() && !isArmourTriggered())
            {
                return new ActionOutcome[] {};
            }

            List<ActionOutcome> outcomes = new();

            var preemptors = potentialPreemptors.Where(a => !a.Id().Equals(preemptor.Id())).ToArray();

            if (isWeaponTriggered())
            {
                outcomes.Add(
                    new(
                        preemptor.Id() as AgentId, 
                        new AgentId[] {}, 
                        ActionType.PreemptTriggered, 
                        new ActionEffect[] { new PreemptTriggered(preemptor.Id() as AgentId, preemptor.RightHand.Type, action)}
                    )
                );

                var outcome = preemptor.RightHand.GetPreExecutionEffects(role, action, preemptor, actor, targets, battle, unitOfWork);
                var effectTargets = outcome.On.Select(i => unitOfWork.AgentRepository.Get(i)).ToArray();
                var weaponOutcomes = preemptors
                .SelectMany(p => Execute(p, preemptors, outcome.Cause, preemptor, effectTargets, battle, unitOfWork));
                
                outcomes.AddRange(weaponOutcomes);

                if (isWeaponTriggered())
                {
                    ApplyActionOutcomeService.Execute(outcome, unitOfWork);
                    outcomes.Add(outcome);
                }
                else
                {
                    outcomes
                    .Add(
                        new(
                            preemptor.Id() as AgentId, 
                            new AgentId[] {}, 
                            ActionType.PreemptAnnulled, 
                            new ActionEffect[] {new PreemptAnnulled(preemptor.Id() as AgentId, preemptor.RightHand.Type, action)}
                        )
                    );
                }                
            }

            if (isArmourTriggered())
            {
                outcomes
                .Add(
                    new(
                        preemptor.Id() as AgentId, 
                        new AgentId[] {}, 
                        ActionType.PreemptTriggered, 
                        new ActionEffect[] { new PreemptTriggered(preemptor.Id() as AgentId, preemptor.Armour.Type, action)}
                    )
                );

                var outcome = preemptor.Armour.GetPreExecutionEffects(role, action, preemptor, actor, targets, battle, unitOfWork);
                var effectTargets = outcome.On.Select(i => unitOfWork.AgentRepository.Get(i)).ToArray();
                var armourOutcomes = preemptors
                .SelectMany(p => Execute(p, preemptors, outcome.Cause, preemptor, effectTargets, battle, unitOfWork));

                outcomes.AddRange(armourOutcomes);

                if (isArmourTriggered())
                {
                    ApplyActionOutcomeService.Execute(outcome, unitOfWork);
                    outcomes.Add(outcome);
                }
                else
                {
                    outcomes
                    .Add(
                        new(
                            preemptor.Id() as AgentId, 
                            new AgentId[] {}, 
                            ActionType.PreemptAnnulled, 
                            new ActionEffect[] {new PreemptAnnulled(preemptor.Id() as AgentId, preemptor.Armour.Type, action)}
                        )
                    );
                }
            }

            return outcomes.ToArray();
        }
    }
}