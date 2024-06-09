using Common;
namespace Battle
{
    public class ApplyActionOutcomeService
    {
        public static void Execute(ActionOutcome outcome, UnitOfWork unitOfWork)
        {
            foreach(var effect in outcome.Effects)
            {
                effect.Apply(unitOfWork);
            }
        }
    }
}