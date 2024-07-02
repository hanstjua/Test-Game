using Common;

namespace Battle.Footwears
{
    public class IronBoots : Footwear
    {
        public override string Value() => Type.Name;
        public override EquipmentType Type => FootwearType.IronBoots;
        public override string Description => "Hard boots.";
        public override Stats StatsBoost => new(0, 1, 0, 0, 0, 0, 0, 0, 0, 0);

        public override int CalculateResistance(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return 0;
        }
    }
}