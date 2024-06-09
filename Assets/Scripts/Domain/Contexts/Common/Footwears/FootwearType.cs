namespace Battle.Footwears
{
    public class FootwearType : ActionType
    {
        public FootwearType(string name) : base(name) {}

        public static readonly FootwearType IronBoots = new("Iron Boots");
    }
}