using Common;

namespace Battle.Armours
{
    public class ArmourType : EquipmentType
    {
        public ArmourType(string name) : base(name) {}

        public static readonly ArmourType LeatherArmour = new("LeatherArmour");
    }
}