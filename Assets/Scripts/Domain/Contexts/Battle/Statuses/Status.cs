using Common;

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

        protected virtual Agent OnAdd(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            return agent;
        }

        protected virtual ActionOutcome[] OnApply(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            return new ActionOutcome[] {};
        }

        protected virtual Agent OnRemove(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            return agent;
        }

        public Agent Add(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            return OnAdd(agent.AddStatus(this), battle, unitOfWork);
        }

        public ActionOutcome[] Apply(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            var outcomes = OnApply(agent, battle, unitOfWork);

            foreach (var outcome in outcomes)
            {
                ApplyActionOutcomeService.Execute(outcome, unitOfWork);
            }

            return outcomes;
        }

        public Agent Remove(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            return OnRemove(agent, battle, unitOfWork);
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