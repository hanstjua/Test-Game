using Common;
using System;
using System.Linq;

namespace Battle.Services.Actions
{
    public class Ether : Action
    {
        public Ether() : base(ActionType.Ether, "Ether.")
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
            if (!targets[0].IsAlive())
            {
                throw new InvalidOperationException("Target is KO'd.");
            }

            actor.ConsumeItem(Item.Ether);
            targets[0].RestoreMp(50);
            var effectOnTarget = new MpDamage((AgentId) targets[0].Id(), -50);

            return new(actor.Id() as AgentId, targets.Select(t => t.Id() as AgentId).ToArray(), ActionType.Ether, new[] { effectOnTarget});
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