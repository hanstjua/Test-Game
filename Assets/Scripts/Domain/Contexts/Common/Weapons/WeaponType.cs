namespace Battle.Weapons
{
    public class WeaponType : HandheldType
    {
        public WeaponType(string name) : base(name) {}

        public static readonly WeaponType Longsword = new("Longsword");
    }
}