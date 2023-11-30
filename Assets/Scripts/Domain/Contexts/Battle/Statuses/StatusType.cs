using Battle;

namespace Battle.Statuses
{
    public class StatusType : ActionType
    {
        public StatusType(string name) : base(name) {}
        public static readonly StatusType Poison = new("Poison");
        public static readonly StatusType Guard = new("Guard");
        public static readonly StatusType KO = new("KO");
    }
}