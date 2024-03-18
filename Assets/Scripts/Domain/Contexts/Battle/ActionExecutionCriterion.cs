namespace Battle
{
    public abstract class ActionExecutionCriterion : ValueObject<string>
    {
        public string Name { get; private set; }

        public ActionExecutionCriterion(string name)
        {
            Name = name;
        }

        public override string Value()
        {
            return Name;
        }

        public abstract bool IsFulfilledBy(Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork);
    }
}