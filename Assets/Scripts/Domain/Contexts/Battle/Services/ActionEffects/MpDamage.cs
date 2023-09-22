namespace Battle
{
    public class MpDamage : ActionEffect
    {
        public int Amount { get; private set; }

        public MpDamage(AgentId on, int amount) : base(on)
        {
            Amount = amount;
        }

        public override string Name => "MpDamage";

        public override string Value()
        {
            return string.Format("{0}", (On.Value(), Name, Amount));
        }

        public override void Apply(UnitOfWork unitOfWork)
        {
            throw new System.NotImplementedException();
        }
    }
}