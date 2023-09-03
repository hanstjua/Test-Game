using System.Linq;
using Battle.Statuses;

namespace Battle.Common.Weapons
{
    public class Longsword : Weapon
    {
        public override ActionOutcome[] ApplyPreExecutionInitiatorEffects(ActionOutcome[] outcomes, Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return outcomes.Append(
                new ActionOutcome(
                    target.Id() as AgentId,
                    WeaponType.Longsword,
                    new ActionEffect[] {new AddStatus(actor.Id() as AgentId, new Guard(3))}
                )).ToArray();
        }

        public override int CalculateDamage(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return 8;
        }
    }
}