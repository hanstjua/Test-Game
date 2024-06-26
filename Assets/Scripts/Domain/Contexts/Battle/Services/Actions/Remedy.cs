using Common;

namespace Battle.Services.Actions
{
    public class Remedy : Action
    {
        public Remedy() : base(ActionType.Remedy, "Remedy.")
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

        public override ArbellumType Arbellum => ArbellumType.Supplies;
        public override ActionPrerequisite[] Criteria => null;

        public override StatType[] ActorRelevantStats => new StatType[] {};
        public override StatType[] TargetRelevantStats => new StatType[] {};

        protected override ActionOutcome OnExecute(Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return null;
        }

        public override bool IsActorAbleToExecute(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            return true;
        }

        protected override bool ShouldExecute(Agent target, Agent actor)
        {
            return false;
        }
    }
}