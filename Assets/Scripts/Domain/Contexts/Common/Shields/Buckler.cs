using Common;
using Battle.Shields;

namespace Battle.Shieds
{
    public class Buckler : Shield
    {
        public override string Value() => Type.Name;
        public override HandheldType Type => ShieldType.Buckler;
        public override Stats StatsBoost => new(0, 1, 0, 0, 0, 0, 0, 0, 0, 0);

        public override int CalculateResistance(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return 0;
        }
    }
}