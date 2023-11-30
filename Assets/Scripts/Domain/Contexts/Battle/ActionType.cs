namespace Battle
{
    public class ActionType
    {
        public ActionType(string name)
        {
            
        }

        public string Name { get; private set; }

        public static readonly ActionType Attack = new("Attack");
        public static readonly ActionType Defend = new("Defend");
        public static readonly ActionType UseItem = new("UseItem");
    }
}