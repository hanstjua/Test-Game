using Battle.Statuses;

namespace Battle
{
    public class ActionFailed : ActionEffect
    {
        public enum FailureType
        {
            Missed,
            Parry,
            Block,
            Immune
        }

        public FailureType Type { get; private set; }

        public ActionFailed(AgentId on, FailureType type) : base(on)
        {
            Type = type;
        }

        public override string Name => "ActionFailed";

        public override string Value()
        {
            return string.Format("{0}", (On.Value(), Name, (int) Type));
        }
    }
}