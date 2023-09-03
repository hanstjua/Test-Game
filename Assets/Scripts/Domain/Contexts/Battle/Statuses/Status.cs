namespace Battle.Statuses{
    public abstract class Status : ValueObject<(string, int)>
    {
        public Status(string name, int duration)
        {
            Name = name;
            Duration = duration;
        }

        public string Name { get; private set; }
        public int Duration { get; private set; }
        public abstract StatusType Type { get; }

        public override (string, int) Value()
        {
            return (Name, Duration);
        }

        public virtual Agent OnAdd(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            return agent;
        }

        public virtual ActionOutcome[] OnApply(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            return new ActionOutcome[] {};
        }

        public virtual Agent OnRemove(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            return agent;
        }

        public Status ProlongBy(int turns)
        {
            Duration += turns;

            return this;
        }

        public Status WearOffBy(int turns)
        {
            Duration = Duration > turns ? Duration - turns : 0;

            return this;
        }
    }
}