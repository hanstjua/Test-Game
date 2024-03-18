namespace Battle
{
    public class PreemptTriggered : ActionEffect
    {
        public ActionType Action { get; private set; }
        public ActionType Cause { get; private set; }

        public PreemptTriggered(AgentId on, ActionType action, ActionType cause) : base(on)
        {
            Action = action;
            Cause = cause;
        }

        public override string Name => "PreemptTriggered";

        public override string Value()
        {
            return string.Format("{0}", (On.Value(), Name));
        }

        public override void Apply(UnitOfWork unitOfWork)
        {
        }
    }
}