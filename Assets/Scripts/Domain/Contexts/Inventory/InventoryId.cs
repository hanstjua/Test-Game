namespace Inventory
{
    public class InventoryId : ValueObject<string>
    {
        public InventoryId(string uuid)
        {
            Uuid = uuid;
        }

        public string Uuid { get; private set; }
        public override string Value()
        {
            return Uuid;
        }

        public override string ToString()
        {
            return Value();
        }
    }
}
