using System.Linq;

namespace Battle.Common.Armours
{
    public class LeatherArmour : Armour
    {
        public override ActionOutcome[] ApplyPostExecutionReactorEffects(ActionOutcome[] outcomes, Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return outcomes.Append(
                new ActionOutcome(
                    target.Id() as AgentId,
                    ArmourType.LeatherArmour,
                    new ActionEffect[] {new HpDamage(target.Id() as AgentId, -3)}
                )).ToArray();
        }


        public override int CalculateResistance(Agent attacker, Agent defender, Battle battle, UnitOfWork unitOfWork)
        {
            return 3;
        }
    }
}