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
    }
}