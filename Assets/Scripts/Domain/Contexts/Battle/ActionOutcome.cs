namespace Battle
{
    public class ActionOutcome
    {
        public AgentId By { get; private set; }
        public AgentId[] On { get; private set; }
        public ActionType Cause { get; private set; }
        public ActionEffect[] Effects { get; private set; }

        public ActionOutcome(AgentId by, AgentId[] on, ActionType cause, ActionEffect[] effects)
        {
            By = by;
            On = on;
            Cause = cause;
            Effects = effects;
        }
    }
}