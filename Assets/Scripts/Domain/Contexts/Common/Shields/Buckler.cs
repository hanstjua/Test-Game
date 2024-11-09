using Common;
using Battle.Shields;
using Battle.Statuses;

namespace Battle.Shieds
{
    public class Buckler : Shield
    {
        public override string Value() => Type.Name;
        public override EquipmentType Type => ShieldType.Buckler;
        public override string Description => "Round {{shield}}.";
        public override Stats StatsBoost => new(0, 1, 0, 0, 0, 0, 0, 0, 50, -50);
        public override ElementType[] Elements => new[] {ElementType.Fire};
        public override StatusType[] Statuses => new[] {StatusType.Guard};

        public override int CalculateResistance(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return 0;
        }
    }
}