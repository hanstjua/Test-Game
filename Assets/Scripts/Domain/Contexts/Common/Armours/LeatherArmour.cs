using System.Linq;
using Battle.Statuses;

namespace Battle.Common.Armours
{
    public class LeatherArmour : Armour
    {
        public override ArmourType Type => ArmourType.LeatherArmour;
        protected override bool IsPostExecutionReactorEffectsTriggered(ActionOutcome outcome, Agent xactor, Agent holder, Battle battle, UnitOfWork unitOfWork)
        {
            return outcome.By.Equals(holder.Id() as AgentId) && holder.IsAlive();
        }

        protected override ActionOutcome OnApplyPostExecutionReactorEffects(ActionOutcome outcome, Agent actor, Agent holder, Battle battle, UnitOfWork unitOfWork)
        {
            return new ActionOutcome(
                holder.Id() as AgentId,
                new AgentId[] {holder.Id() as AgentId},
                ArmourType.LeatherArmour,
                new ActionEffect[] {new HpDamage(holder.Id() as AgentId, -3)}
            );
        }

        protected override bool IsPreExecutionObserverEffectsTriggered(ActionType action, Agent holder, Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return holder.IsAlive() && action == ActionType.Attack;
        }

        protected override ActionOutcome OnApplyPreExecutionObserverEffects(ActionType action, Agent holder, Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return new ActionOutcome(
                holder.Id() as AgentId,
                new AgentId[] {holder.Id() as AgentId},
                ArmourType.LeatherArmour,
                new ActionEffect[] {new AddStatus(holder.Id() as AgentId, new Guard(1), battle.Id() as BattleId)}
            );
        }

        public override int CalculateResistance(Agent attacker, Agent defender, Battle battle, UnitOfWork unitOfWork)
        {
            return 3 + defender.Stats.Defense;
        }
    }
}