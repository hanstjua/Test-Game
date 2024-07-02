using Battle;
using Battle.Armours;
using Common;

#nullable enable

public class ArmourSlot : EquipmentSlot
{
    public override EquipmentType Type => new ArmourType("");

    public override EquipmentSlot TriggerEquipService(AgentId id, Equipment? equipment, UnitOfWork unitOfWork)
    {
        new EquipService().Execute(id, (Armour?) equipment, unitOfWork);
        return this;
    }
}