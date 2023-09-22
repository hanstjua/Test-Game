namespace Battle.Statuses
{
    public class Guard : Status
    {
        public Guard(int duration): base("Guard", duration) {}
        
        public override StatusType Type => StatusType.Guard;

        protected override Agent OnAdd(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            var agentDefense = agent.Stats.Defense;
            return agent.UpdateStats(agent.Stats.ModifyStat(Stats.Type.Defense, agentDefense + 3));
        }

        protected override Agent OnRemove(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            var agentDefense = agent.Stats.Defense;
            return agent.UpdateStats(agent.Stats.ModifyStat(Stats.Type.Defense, agentDefense - 3));
        }
    }
}