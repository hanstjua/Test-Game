using System;

namespace Battle.Statuses
{
    public class Poison : Status
    {
        public Poison(int duration): base("Poison", duration) {}
        
        public override StatusType Type => StatusType.Poison;

        protected override ActionOutcome[] OnApply(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            return new ActionOutcome[] {
                new ActionOutcome(
                    agent.Id() as AgentId,
                    new AgentId[] {agent.Id() as AgentId},
                    Type,
                    new ActionEffect[] {
                        new HpDamage(agent.Id() as AgentId, (int) Math.Ceiling(agent.Stats.MaxHp * 0.1))
                    }
                )
            };
        }
    }
}