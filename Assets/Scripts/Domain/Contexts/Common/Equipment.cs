namespace Battle.Common
{
    public class Equipment
    {
        public virtual ActionOutcome[] ApplyPreExecutionInitiatorEffects(ActionOutcome[] queueOutcomes, Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return queueOutcomes;
        }

        public virtual ActionOutcome[] ApplyPostExecutionInitiatorEffects(ActionOutcome[] queuedOutcomes, Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return queuedOutcomes;
        }

        public virtual ActionOutcome[] ApplyPreExecutionReactorEffects(ActionOutcome[] queuedOutcomes, Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return queuedOutcomes;
        }

        public virtual ActionOutcome[] ApplyPostExecutionReactorEffects(ActionOutcome[] queuedOutcomes, Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return queuedOutcomes;
        }

        public virtual ActionOutcome[] ApplyPreExecutionObserverEffects(ActionOutcome[] queuedOutcomes, Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return queuedOutcomes;
        }

        public virtual ActionOutcome[] ApplyPostExecutionObserverEffects(ActionOutcome[] queuedOutcomes, Agent actor, Agent target, Battle battle, UnitOfWork unitOfWork)
        {
            return queuedOutcomes;
        }

    }
}