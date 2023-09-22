using Battle.Common;

namespace Battle
{
    public class ItemLost : ActionEffect
    {
        public Item Item { get; private set; }
        public int Quantity { get; private set; }

        public ItemLost(AgentId on, Item item, int quantity) : base(on)
        {
            Item = item;
            Quantity = quantity;
        }

        public override string Name => "ItemLost";

        public override string Value()
        {
            return string.Format("{0}", (On.Value(), Name, Item, Quantity));
        }

        public override void Apply(UnitOfWork unitOfWork)
        {
            throw new System.NotImplementedException();
        }
    }
}