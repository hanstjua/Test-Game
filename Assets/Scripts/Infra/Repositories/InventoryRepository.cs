using Inventory;
using System;

public class InventoryRepository : IInventoryRepository
{
    private byte[] _savedInventory;
    private Inventory.Inventory _current = new(new InventoryId(Guid.NewGuid().ToString()));
    public Inventory.Inventory Get() => _current;

    public IInventoryRepository Reload()
    {
        _current = Serializer.Deserialize<Inventory.Inventory>(_savedInventory);
        return this;
    }

    public IInventoryRepository Save()
    {
        _savedInventory = Serializer.Serialize(_current);
        return this;
    }

    public Inventory.Inventory Update(Inventory.Inventory inventory)
    {
        _current = inventory;
        return inventory;
    }
}