using Battle;
using Battle.Footwears;
using Common;

#nullable enable

public class FootwearSlot : EquipmentSlot
{
    public override EquipmentType Type => new FootwearType("");

    public override EquipmentSlot TriggerEquipService(AgentId id, Equipment? equipment, UnitOfWork unitOfWork)
    {
        new EquipService().Execute(id, (Footwear?) equipment, unitOfWork);
        return this;
    }
}