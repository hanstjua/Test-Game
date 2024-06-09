namespace Battle.Shields
{
    public class ShieldType : HandheldType
    {
        public ShieldType(string name) : base(name) {}

        public static readonly ShieldType Buckler = new("Buckler");
    }
}