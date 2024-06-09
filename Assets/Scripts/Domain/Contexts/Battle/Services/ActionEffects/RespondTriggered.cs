using Common;

namespace Battle
{
    public class RespondTriggered : ActionEffect
    {
        public ActionType Action { get; private set; }
        public ActionOutcome Cause { get; private set; }

        public RespondTriggered(AgentId on, ActionType action, ActionOutcome cause) : base(on)
        {
            Action = action;
            Cause = cause;
        }

        public override string Name => "RespondTriggered";

        public override string Value()
        {
            return string.Format("{0}", (On.Value(), Name));
        }

        public override void Apply(UnitOfWork unitOfWork)
        {
        }
    }
}