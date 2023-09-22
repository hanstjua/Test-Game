using System.Linq;

namespace Battle.Common.Armours
{
    public class LeatherArmour : Armour
    {
        protected override ActionOutcome OnApplyPostExecutionReactorEffects(ActionOutcome outcome, Agent actor, Agent holder, Battle battle, UnitOfWork unitOfWork)
        {
            return holder.IsAlive() ? new ActionOutcome(
                holder.Id() as AgentId,
                new AgentId[] {holder.Id() as AgentId},
                ArmourType.LeatherArmour,
                new ActionEffect[] {new HpDamage(holder.Id() as AgentId, -3)}
            ) : null;
        }


        public override int CalculateResistance(Agent attacker, Agent defender, Battle battle, UnitOfWork unitOfWork)
        {
            return 3;
        }
    }
}