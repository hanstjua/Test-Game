using Battle;
using Common;

#nullable enable

public class LeftHandSlot : EquipmentSlot
{
    public override EquipmentType Type => new HandheldType("");

    public override EquipmentSlot TriggerEquipService(AgentId id, Equipment? equipment, UnitOfWork unitOfWork)
    {
        new EquipService().Execute(id, (Handheld?) equipment, false, unitOfWork);
        return this;
    }
}