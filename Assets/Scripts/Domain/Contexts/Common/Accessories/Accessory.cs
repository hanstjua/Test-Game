using Common;
namespace Battle.Accessories
{
    public abstract class Accessory : Equipment
    {
        public abstract AccessoryType Type { get; }
        public abstract int CalculateResistance(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork);
    }
}