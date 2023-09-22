namespace Battle
{
    public class HpDamage : ActionEffect
    {
        public int Amount { get; private set; }

        public HpDamage(AgentId on, int amount) : base(on)
        {
            Amount = amount;
        }

        public override string Name => "HpDamage";

        public override string Value()
        {
            return string.Format("{0}", (On.Value(), Name, Amount));
        }

        public override void Apply(UnitOfWork unitOfWork)
        {
            using (unitOfWork)
            {
                var agent = unitOfWork.AgentRepository.Get(On);
                agent = Amount > 0 ? agent.ReduceHp(Amount) : agent.RestoreHp(Amount);
                unitOfWork.AgentRepository.Update(agent.Id() as AgentId, agent);
                unitOfWork.Save();
            }
        }
    }
}