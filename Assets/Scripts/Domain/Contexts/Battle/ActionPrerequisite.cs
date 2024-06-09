using Common;

namespace Battle
{
    public abstract class ActionPrerequisite : ValueObject<string>
    {
        public string Name { get; private set; }

        public ActionPrerequisite(string name)
        {
            Name = name;
        }

        public override string Value()
        {
            return Name;
        }

        public abstract bool IsFulfilledBy(Agent actor, Battle battle, UnitOfWork unitOfWork);
    }
}