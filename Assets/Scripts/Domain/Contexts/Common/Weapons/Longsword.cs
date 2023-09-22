using System.Linq;
using Battle.Statuses;

namespace Battle.Common.Weapons
{
    public class Longsword : Weapon
    {
        public override WeaponType Type => WeaponType.Longsword;
        protected override ActionOutcome OnApplyPreExecutionInitiatorEffects(ActionType action, Agent holder, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return new ActionOutcome(
                holder.Id() as AgentId,
                new AgentId[] {holder.Id() as AgentId},
                WeaponType.Longsword,
                new ActionEffect[] {new AddStatus(holder.Id() as AgentId, new Guard(3), battle.Id() as BattleId)}
            );
        }

        public override int CalculateDamage(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return 8;
        }
    }
}