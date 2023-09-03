using Battle.Statuses;

namespace Battle
{
    public class AddStatus : ActionEffect
    {
        public Status Status { get; private set; }

        public AddStatus(AgentId on, Status status) : base(on)
        {
            Status = status;
        }

        public override string Name => "AddStatus";

        public override string Value()
        {
            return string.Format("{0}", (On.Value(), Name, Status.Value()));
        }
    }
}