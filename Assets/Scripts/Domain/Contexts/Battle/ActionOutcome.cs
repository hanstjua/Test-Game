namespace Battle
{
    public class ActionOutcome
    {
        public AgentId By { get; private set; }
        public ActionType Cause { get; private set; }
        public ActionEffect[] Effects { get; private set; }

        public ActionOutcome(AgentId by, ActionType cause, ActionEffect[] effects)
        {
            By = by;
            Cause = cause;
            Effects = effects;
        }
    }
}