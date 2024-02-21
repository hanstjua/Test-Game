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
        public static readonly ActionType Fire = new("Fire");
        public static readonly ActionType Water = new("Water");
        public static readonly ActionType Ice = new("Ice");
        public static readonly ActionType Thunder = new("Thunder");
        public static readonly ActionType Wind = new("Wind");
        public static readonly ActionType Earth = new("Earth");
    }
}