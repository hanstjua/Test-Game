using Battle.Services.ActionPrerequisites;

namespace Battle.Services.Actions
{
    public class Disintegrate : Action
    {
        public Disintegrate() : base(ActionType.Disintegrate, "A Magic attack that launches a daya projectile that renders unregulated daya it cuts through into a destructive force, damaging the flesh where the daya resided.")
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
        public override int Cost => 9999;

        public override ArbellumType Arbellum => ArbellumType.Malediction;
        public override ActionPrerequisite[] Criteria => new[] { new NotParalyzed() };

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
            return true;
        }
    }
}