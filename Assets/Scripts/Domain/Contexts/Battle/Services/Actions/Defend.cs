using System.Linq;
using Battle.Services.ActionCriteria;
using Battle.Statuses;
using UnityEditor.Media;

namespace Battle.Services.Actions
{
    public class Defend : Action
    {
        public Defend() : base("Defend", "Take defensive stance to guard against incoming attacks.")
        {
        }

        public override AreaOfEffect AreaOfEffect => new AreaOfEffect(
            new Position[] {new Position(0, 0, 0)},
            0
        );
        public override ActionType Type => ActionType.Defend;

        public override SkillType Skill => SkillType.Physical;
        public override ActionCriterion[] Criteria => new[] { new NotParalyzed() };

        protected override ActionOutcome OnExecute(Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return new ActionOutcome(
                actor.Id() as AgentId,
                new AgentId[] {actor.Id() as AgentId},
                Type,
                new ActionEffect[] {
                    new AddStatus(actor.Id() as AgentId, new Guard(3), battle.Id() as BattleId)
                }
            );
        }

        protected override bool ShouldExecute(Agent target, Agent actor)
        {
            return actor.IsAlive();
        }
    }
}