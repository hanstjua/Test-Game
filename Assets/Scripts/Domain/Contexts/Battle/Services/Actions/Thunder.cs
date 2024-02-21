using System.Linq;
using Battle.Services.ActionCriteria;
using Battle.Statuses;
using UnityEditor.Media;

namespace Battle.Services.Actions
{
    public class Thunder : Action
    {
        public Thunder() : base("Thunder", "Roar.")
        {
        }

        public override AreaOfEffect AreaOfEffect => new AreaOfEffect(
            new Position[] {new Position(0, 0, 0)},
            0
        );
        public override ActionType Type => ActionType.Thunder;

        public override SkillType Skill => SkillType.Physical;
        public override ActionCriterion[] Criteria => new[] { new NotParalyzed() };

        protected override ActionOutcome OnExecute(Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return null;
        }

        protected override bool ShouldExecute(Agent target, Agent actor)
        {
            return false;
        }
    }
}