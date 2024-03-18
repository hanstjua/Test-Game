namespace Battle
{
    public class PreemptAnnulled : ActionEffect
    {
        public ActionType Action { get; private set; }
        public ActionType Cause { get; private set; }

        public PreemptAnnulled(AgentId on, ActionType action, ActionType cause) : base(on)
        {
            Action = action;
            Cause = cause;
        }

        public override string Name => "PreemptAnnulled";

        public override string Value()
        {
            return string.Format("{0}", (On.Value(), Name));
        }

        public override void Apply(UnitOfWork unitOfWork)
        {
        }
    }
}