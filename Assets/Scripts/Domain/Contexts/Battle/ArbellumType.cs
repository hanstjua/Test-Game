namespace Battle
{
    public class ArbellumType
    {
        public ArbellumType(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public static readonly ArbellumType Physical = new("Physical");
        public static readonly ArbellumType Supplies = new("Supplies");
        public static readonly ArbellumType Pyrecraft = new("Pyrecraft");
        public static readonly ArbellumType Glaciocraft = new("Glaciocraft");
        public static readonly ArbellumType Brontocraft = new("Brontocraft");
        public static readonly ArbellumType Hydrocraft = new("Hydrocraft");
        public static readonly ArbellumType Geocraft = new("Geocraft");
        public static readonly ArbellumType Aercraft = new("Aercraft");
        public static readonly ArbellumType Profanity = new("Profanity");
        public static readonly ArbellumType Divinity = new("Divinity");
        public static readonly ArbellumType Viscraft = new("Viscraft");
        public static readonly ArbellumType Horology = new("Horology");
        public static readonly ArbellumType Malediction = new("Malediction");
        public static readonly ArbellumType Physics = new("Physics");
        public static readonly ArbellumType Numerics = new("Numerics");
    }
}