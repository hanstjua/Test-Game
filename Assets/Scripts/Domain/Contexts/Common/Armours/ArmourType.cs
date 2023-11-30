namespace Battle.Common.Armours
{
    public class ArmourType : ActionType
    {
        public ArmourType(string name) : base(name) {}

        public static readonly ArmourType LeatherArmour = new("LeatherArmour");
    }
}