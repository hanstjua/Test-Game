using Battle;
using Battle.Accessories;
using Battle.Armours;
using Battle.Footwears;
using Common;

#nullable enable

public class EquipService
{
    public void Execute(AgentId id, Handheld? handheld, bool right, UnitOfWork unitOfWork)
    {
        var agent = unitOfWork.AgentRepository.Get(id);
        var oldEquipment = right ? agent.RightHand : agent.LeftHand;

        if (oldEquipment != handheld)
        {
            var inventory = unitOfWork.InventoryRepository.Get();

            using (unitOfWork)
            {
                inventory = handheld != null ? unitOfWork.InventoryRepository.Update(inventory.Equip(handheld)) : inventory;
                inventory = oldEquipment != null ? unitOfWork.InventoryRepository.Update(inventory.Unequip(oldEquipment)) : inventory;

                var newAgent = right ? agent.RightHandEquip(handheld) : agent.LeftHandEquip(handheld);
                unitOfWork.AgentRepository.Update(id, newAgent);

                unitOfWork.Save();
            }
        }
    }

    public void Execute(AgentId id, Armour? armour, UnitOfWork unitOfWork)
    {
        var agent = unitOfWork.AgentRepository.Get(id);
        var oldEquipment = agent.Armour;

        if (oldEquipment != armour)
        {
            var inventory = unitOfWork.InventoryRepository.Get();

            using (unitOfWork)
            {
                inventory = armour != null ? unitOfWork.InventoryRepository.Update(inventory.Equip(armour)) : inventory;
                inventory = oldEquipment != null ? unitOfWork.InventoryRepository.Update(inventory.Unequip(oldEquipment)) : inventory;

                unitOfWork.AgentRepository.Update(id, agent.ArmourEquip(armour));

                unitOfWork.Save();
            }
        }
    }

    public void Execute(AgentId id, Footwear? footwear, UnitOfWork unitOfWork)
    {
        var agent = unitOfWork.AgentRepository.Get(id);
        var oldEquipment = agent.Footwear;

        if (oldEquipment != footwear)
        {
            var inventory = unitOfWork.InventoryRepository.Get();

            using (unitOfWork)
            {
                inventory = footwear != null ? unitOfWork.InventoryRepository.Update(inventory.Equip(footwear)) : inventory;
                inventory = oldEquipment != null ? unitOfWork.InventoryRepository.Update(inventory.Unequip(oldEquipment)) : inventory;

                unitOfWork.AgentRepository.Update(id, agent.FootwearEquip(footwear));

                unitOfWork.Save();
            }
        }
    }

    public void Execute(AgentId id, Accessory? accessory, bool first, UnitOfWork unitOfWork)
    {
        var agent = unitOfWork.AgentRepository.Get(id);
        var oldEquipment = first ? agent.Accessory1 : agent.Accessory2;

        if (oldEquipment != accessory)
        {
            var inventory = unitOfWork.InventoryRepository.Get();

            using (unitOfWork)
            {
                inventory = accessory != null ? unitOfWork.InventoryRepository.Update(inventory.Equip(accessory)) : inventory;
                inventory = oldEquipment != null ? unitOfWork.InventoryRepository.Update(inventory.Unequip(oldEquipment)) : inventory;

                var newAgent = first ? agent.Accessory1Equip(accessory) : agent.Accessory2Equip(accessory);
                unitOfWork.AgentRepository.Update(id, newAgent);

                unitOfWork.Save();
            }
        }
    }
}

#nullable disable