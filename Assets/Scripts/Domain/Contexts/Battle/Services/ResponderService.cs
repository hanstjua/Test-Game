using System;
using System.Linq;
using Battle.Common;

namespace Battle
{
    public class RespondService
    {
        public ActionOutcome[] Execute(Agent responder, Agent[] potentialResponders, ActionOutcome outcome, Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {

            var role = Equipment.GetHolderRole(responder.Id() as AgentId, actor.Id() as AgentId, targets.Select(a => a.Id() as AgentId).ToArray());

            Func<bool> isWeaponTriggered = () => responder.Weapon.IsPostExecutionEffectsTriggered(role, outcome, responder, actor, targets, battle, unitOfWork);
            Func<bool> isArmourTriggered = () => responder.Armour.IsPostExecutionEffectsTriggered(role, outcome, responder, actor, targets, battle, unitOfWork);

            ActionOutcome[] ret = {};

            if (!isWeaponTriggered() && !isArmourTriggered())
            {
                return ret;
            }

            var responders = potentialResponders.Where(a => !a.Id().Equals(responder.Id())).ToArray();

            if (isWeaponTriggered())
            {
                var weaponOutcome = responder.Weapon.GetPostExecutionEffects(role, outcome, responder, actor, targets, battle, unitOfWork);
                ApplyActionOutcomeService.Execute(weaponOutcome, unitOfWork);
                ret = ret.Append(weaponOutcome).ToArray();

                var effectTargets = weaponOutcome.On.Select(i => unitOfWork.AgentRepository.Get(i)).ToArray();
                var weaponOutcomes = responders
                .Aggregate(new ActionOutcome[] {}, 
                (outcomes, responder) => outcomes.Concat(Execute(responder, responders, weaponOutcome, responder, effectTargets, battle, unitOfWork)).ToArray())
                .ToArray();
                
                ret = ret.Concat(weaponOutcomes).ToArray();
            }

            if (isArmourTriggered())
            {
                var armourOutcome = responder.Armour.GetPostExecutionEffects(role, outcome, responder, actor, targets, battle, unitOfWork);
                ApplyActionOutcomeService.Execute(armourOutcome, unitOfWork);
                ret = ret.Append(armourOutcome).ToArray();

                var effectTargets = outcome.On.Select(i => unitOfWork.AgentRepository.Get(i)).ToArray();
                var armourOutcomes = responders
                .Aggregate(new ActionOutcome[] {}, 
                (outcomes, preemptor) => outcomes.Concat(Execute(responder, responders, armourOutcome, responder, effectTargets, battle, unitOfWork)).ToArray())
                .ToArray();

                ret = ret.Concat(armourOutcomes).ToArray();
            }

            return ret;
        }
    }
}