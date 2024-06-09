using Battle;
using Battle.Armours;
using Battle.Weapons;
using System;
using System.Linq;

class ExecutionStateDispatcher
{
    public static IUiState Dispatch(ActionOutcome[] outcomes, IUiState nextState)
    {
        var _nextState = outcomes.Count() == 1 ? nextState : Dispatch(outcomes.Skip(1).ToArray(), nextState);

        var cause = outcomes[0].Cause;

        if (cause == ActionType.PreemptTriggered) return new PreemptTriggeredExecution(outcomes[0], _nextState);
        if (cause == ActionType.RespondTriggered) return new RespondTriggeredExecution(outcomes[0], _nextState);

        if (cause == ActionType.Attack) return new AttackExecution(outcomes[0], _nextState);
        if (cause == ActionType.Defend) return new DefendExecution(outcomes[0], _nextState);
        if (cause == WeaponType.Longsword) return new LongswordExecution(outcomes[0], _nextState);
        if (cause == ArmourType.LeatherArmour) return new LeatherArmourExecution(outcomes[0], _nextState);
        
        throw new InvalidOperationException(string.Format("Unhandled cause {0}", cause.GetType().Name));
    }
}