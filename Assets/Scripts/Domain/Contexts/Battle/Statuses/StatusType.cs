using Battle;

namespace Battle.Statuses
{
    public class StatusType : ActionType
    {
        public static readonly StatusType Poison = new();
        public static readonly StatusType Guard = new();
        public static readonly StatusType KO = new();
    }
}