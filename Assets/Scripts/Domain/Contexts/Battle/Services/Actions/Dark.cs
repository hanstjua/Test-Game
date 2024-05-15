using Battle.Services.ActionPrerequisites;
using Battle.Statuses;

namespace Battle.Services.Actions
{
    public class Dark : Action
    {
        public Dark() : base(ActionType.Dark, "Shoong.")
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
        
        public override ArbellumType Arbellum => ArbellumType.Profanity;
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
            return false;
        }
    }
}