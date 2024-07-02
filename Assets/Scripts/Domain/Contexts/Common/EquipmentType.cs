using Battle;

namespace Common
{
    public abstract class EquipmentType : ActionType
    {
        public EquipmentType(string name) : base(name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}