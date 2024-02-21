using System;

namespace Battle
{
    public abstract class ActionCriterion : ValueObject<string>
    {
        public string Name { get; private set; }

        public ActionCriterion(string name)
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