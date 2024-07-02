using Common;
namespace Battle.Accessories
{
    public abstract class Accessory : Equipment
    {
        public abstract int CalculateResistance(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork);
    }
}