using System.Collections.Generic;
using System.Linq;
using Battle.Common;

namespace Battle
{
    public class RespondService
    {
        public ActionOutcome[] Execute(Agent responder, Agent[] potentialResponders, ActionOutcome outcome, Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {

            var role = Equipment.GetHolderRole(responder.Id() as AgentId, actor.Id() as AgentId, targets.Select(a => a.Id() as AgentId).ToArray());

            bool isWeaponTriggered() => responder.Weapon.IsPostExecutionEffectsTriggered(role, outcome, responder, actor, targets, battle, unitOfWork);
            bool isArmourTriggered() => responder.Armour.IsPostExecutionEffectsTriggered(role, outcome, responder, actor, targets, battle, unitOfWork);

            List<ActionOutcome> outcomes = new();

            if (!isWeaponTriggered() && !isArmourTriggered())
            {
                return new ActionOutcome[] {};
            }

            var responders = potentialResponders.Where(a => !a.Id().Equals(responder.Id())).ToArray();

            if (isWeaponTriggered())
            {
                outcomes.Add(
                    new(
                        responder.Id() as AgentId, 
                        new AgentId[] {}, 
                        ActionType.RespondTriggered, 
                        new ActionEffect[] { new RespondTriggered(responder.Id() as AgentId, responder.Weapon.Type, outcome)}
                    )
                );

                var weaponOutcome = responder.Weapon.GetPostExecutionEffects(role, outcome, responder, actor, targets, battle, unitOfWork);
                ApplyActionOutcomeService.Execute(weaponOutcome, unitOfWork);
                outcomes.Add(weaponOutcome);

                var effectTargets = weaponOutcome.On.Select(i => unitOfWork.AgentRepository.Get(i)).ToArray();
                var weaponOutcomes = responders
                .SelectMany(r => Execute(r, responders, weaponOutcome, responder, effectTargets, battle, unitOfWork));
                
                outcomes.AddRange(weaponOutcomes);
            }

            if (isArmourTriggered())
            {
                outcomes.Add(
                    new(
                        responder.Id() as AgentId, 
                        new AgentId[] {}, 
                        ActionType.RespondTriggered, 
                        new ActionEffect[] { new RespondTriggered(responder.Id() as AgentId, responder.Armour.Type, outcome)}
                    )
                );

                var armourOutcome = responder.Armour.GetPostExecutionEffects(role, outcome, responder, actor, targets, battle, unitOfWork);
                ApplyActionOutcomeService.Execute(armourOutcome, unitOfWork);
                outcomes.Add(armourOutcome);

                var effectTargets = outcome.On.Select(i => unitOfWork.AgentRepository.Get(i)).ToArray();
                var armourOutcomes = responders
                .SelectMany(r => Execute(r, responders, armourOutcome, responder, effectTargets, battle, unitOfWork));

                outcomes.AddRange(armourOutcomes);
            }

            return outcomes.ToArray();
        }
    }
}