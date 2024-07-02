using Common;

namespace Battle.Footwears
{
    public class FootwearType : EquipmentType
    {
        public FootwearType(string name) : base(name) {}

        public static readonly FootwearType IronBoots = new("Iron Boots");
    }
}