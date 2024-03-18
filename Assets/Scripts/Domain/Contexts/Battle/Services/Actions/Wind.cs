using System.Linq;
using Battle.Services.ActionPrerequisites;
using Battle.Statuses;
using UnityEditor.Media;

namespace Battle.Services.Actions
{
    public class Wind : Action
    {
        public Wind() : base("Wind", "Whoosh.")
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
        public override ActionType Type => ActionType.Wind;

        public override SkillType Skill => SkillType.Physical;
        public override ActionPrerequisite[] Criteria => new[] { new NotParalyzed() };

        protected override ActionOutcome OnExecute(Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return null;
        }

        public override bool CanExecute(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            return true;
        }

        protected override bool ShouldExecute(Agent target, Agent actor)
        {
            return false;
        }
    }
}