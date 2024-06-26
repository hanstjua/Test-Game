using System;
using System.Linq;
using Battle;
using Battle.Services.ActionPrerequisites;
using Common;

namespace Battle.Services.Actions
{
    public class UseItem : Action
    {
        public UseItem() : base(ActionType.UseItem, "Use item.")
        {
        }

        public override AreaOfEffect AreaOfEffect => new(
            new Position[] {new(0,0,0)},
            1
        );

        public override AreaOfEffect TargetArea => throw new NotImplementedException();

        public override ArbellumType Arbellum => ArbellumType.Supplies;
        public override ActionPrerequisite[] Criteria => new[] { new NotParalyzed() };

        public override StatType[] ActorRelevantStats => new StatType[] {};
        public override StatType[] TargetRelevantStats => new StatType[] {};

        public ActionEffect[] Execute(Agent actor, Agent target, Item item)
        {
            return null;
        }

        protected override ActionOutcome OnExecute(Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            throw new NotImplementedException();
        }

        public override bool IsActorAbleToExecute(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            return true;
        }

        protected override bool ShouldExecute(Agent target, Agent actor)
        {
            throw new NotImplementedException();
        }
    }
}