namespace Battle.Common.Armours
{
    public interface IArmour
    {
        public int CalculateResistance(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork);
        public ActionOutcome[] CalculateSideEffects(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork);
    }
}