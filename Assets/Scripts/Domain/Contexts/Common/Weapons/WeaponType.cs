namespace Battle.Common.Weapons
{
    public class WeaponType : ActionType
    {
        public WeaponType(string name) : base(name) {}

        public static readonly WeaponType Longsword = new("Longsword");
    }
}