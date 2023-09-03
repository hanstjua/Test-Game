using Battle.Statuses;

namespace Battle
{
    public class RemoveStatus : ActionEffect
    {
        public Status Status { get; private set; }

        public RemoveStatus(AgentId on, Status status) : base(on)
        {
            Status = status;
        }

        public override string Name => "RemoveStatus";

        public override string Value()
        {
            return string.Format("{0}", (On.Value(), Name, Status.Value()));
        }
    }
}