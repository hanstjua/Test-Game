namespace Inventory
{
    public interface IInventoryRepository
    {
        public Inventory Get();
        public Inventory Update(Inventory inventory);
        public IInventoryRepository Reload();
        public IInventoryRepository Save();
    }
}