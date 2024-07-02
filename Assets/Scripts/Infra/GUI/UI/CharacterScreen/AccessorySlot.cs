using Battle;
using Battle.Accessories;
using Common;

#nullable enable

public class AccessorySlot : EquipmentSlot
{
    public bool FirstSlot;
    public override EquipmentType Type => new AccessoryType("");

    public override EquipmentSlot TriggerEquipService(AgentId id, Equipment? equipment, UnitOfWork unitOfWork)
    {
        new EquipService().Execute(id, (Accessory?) equipment, FirstSlot, unitOfWork);
        return this;
    }
}