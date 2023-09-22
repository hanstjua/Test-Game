using Battle.Statuses;

namespace Battle
{
    public class RemoveStatus : ActionEffect
    {
        public Status Status { get; private set; }
        public BattleId BattleId { get; private set; }

        public RemoveStatus(AgentId on, Status status, BattleId battleId) : base(on)
        {
            Status = status;
            BattleId = battleId;
        }

        public override string Name => "RemoveStatus";

        public override string Value()
        {
            return string.Format("{0}", (On.Value(), Name, Status.Value()));
        }

        public override void Apply(UnitOfWork unitOfWork)
        {
            using (unitOfWork)
            {
                var agent = unitOfWork.AgentRepository.Get(On);
                agent = Status.Remove(agent, unitOfWork.BattleRepository.Get(BattleId), unitOfWork);
                unitOfWork.AgentRepository.Update(agent.Id() as AgentId, agent);
                unitOfWork.Save();
            }
        }
    }
}