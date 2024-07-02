using Common;
namespace Battle.Accessories
{
    public class GoldNecklace : Accessory
    {
        public override string Value() => Type.Name;
        public override EquipmentType Type => AccessoryType.GoldNecklace;
        public override string Description => "Is it real or fake?";
        public override Stats StatsBoost => new(0, 1, 0, 0, 0, 0, 0, 0, 0, 0);

        public override int CalculateResistance(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return 0;
        }
    }
}