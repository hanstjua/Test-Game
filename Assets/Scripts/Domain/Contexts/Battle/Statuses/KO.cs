namespace Battle.Statuses
{
    public class KO : Status
    {
        public KO(): base("KO", -1) {}
        
        public override StatusType Type => StatusType.KO;

        protected override Agent OnAdd(Agent agent, Battle battle, UnitOfWork unitOfWork)
        {
            return agent.ResetTurnGauge();
        }
    }
}