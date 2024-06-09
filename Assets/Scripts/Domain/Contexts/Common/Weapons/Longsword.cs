using Battle.Statuses;
using Common;

namespace Battle.Weapons
{
    public class Longsword : Weapon
    {
        public override string Value() => Type.Name;
        public override HandheldType Type => WeaponType.Longsword;
        public override Stats StatsBoost => new(3, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        protected override bool IsPostExecutionInitiatorEffectsTriggered(ActionOutcome outcome, Agent holder, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return outcome.By.Equals(holder.Id() as AgentId) && holder.IsAlive();
        }

        protected override ActionOutcome OnApplyPostExecutionInitiatorEffects(ActionOutcome outcome, Agent holder, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return new ActionOutcome(
                holder.Id() as AgentId,
                new AgentId[] {holder.Id() as AgentId},
                WeaponType.Longsword,
                new ActionEffect[] {
                    new AddStatus(holder.Id() as AgentId, new Guard(3), battle.Id() as BattleId),
                    new AddStatus(holder.Id() as AgentId, new Poison(3), battle.Id() as BattleId)
                }
            );
        }

        public override int CalculateDamage(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return 8;
        }
    }
}