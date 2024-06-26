using Common;

namespace Battle.Accessories
{
    public class AccessoryType : EquipmentType
    {
        public AccessoryType(string name) : base(name) {}

        public static readonly AccessoryType GoldNecklace = new("Gold Necklace");
        public static readonly AccessoryType SilverRing = new("Silver Ring");
    }
}