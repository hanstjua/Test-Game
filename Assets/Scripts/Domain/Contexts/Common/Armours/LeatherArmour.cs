namespace Battle.Common.Armours
{
    public class LeatherArmour : IArmour
    {
        public int CalculateResistance(Agent attacker, Agent defender, Battle battle, UnitOfWork unitOfWork)
        {
            return 3;
        }

        public ActionOutcome[] CalculateSideEffects(Agent attacker, Agent defender, Battle battle, UnitOfWork unitOfWork)
        {
            return new ActionOutcome[] {new ActionOutcome(defender.Id() as AgentId, hpDamage: -3)};
        }
    }
}