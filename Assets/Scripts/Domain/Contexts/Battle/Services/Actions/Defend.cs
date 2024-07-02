using Battle.Services.ActionPrerequisites;
using Battle.Statuses;
using Common;

namespace Battle.Services.Actions
{
    public class Defend : Action
    {
        public Defend() : base(ActionType.Defend, "Take defensive stance to guard against incoming attacks.")
        {
        }

        public override AreaOfEffect AreaOfEffect => new(
            new Position[] {new(0,0,0)},
            1
        );

        public override AreaOfEffect TargetArea => new(
            new Position[] {new(0, 0, 0)},
            0
        );

        public override ArbellumType Arbellum => ArbellumType.Physical;
        public override ActionPrerequisite[] Criteria => new[] { new NotParalyzed() };

        public override StatType[] ActorRelevantStats => new StatType[] {};
        public override StatType[] TargetRelevantStats => new StatType[] {};
        public override int Cost => 0;

        protected override ActionOutcome OnExecute(Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return new ActionOutcome(
                actor.Id() as AgentId,
                new AgentId[] {actor.Id() as AgentId},
                Type,
                new ActionEffect[] {
                    new AddStatus(actor.Id() as AgentId, new Guard(1), battle.Id() as BattleId)
                }
            );
        }

        public override bool IsActorAbleToExecute(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            return true;
        }

        protected override bool ShouldExecute(Agent target, Agent actor)
        {
            return actor.IsAlive();
        }
    }
}