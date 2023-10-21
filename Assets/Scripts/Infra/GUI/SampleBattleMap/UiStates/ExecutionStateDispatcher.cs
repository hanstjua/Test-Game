using Battle;
using Battle.Common.Weapons;
using System;
using System.Linq;

class ExecutionStateDispatcher
{
    public static IUiState Dispatch(ActionOutcome[] outcomes, IUiState nextState)
    {
        var _nextState = outcomes.Count() == 1 ? nextState : Dispatch(outcomes.Skip(1).ToArray(), nextState);

        var cause = outcomes[0].Cause;

        if (cause == ActionType.Attack) return new AttackExecution(outcomes[0], _nextState);
        if (cause == WeaponType.Longsword) return new LongswordExecution(outcomes[0], _nextState);
        
        throw new InvalidOperationException(string.Format("Unhandled outcome {0}", outcomes[0].GetType().Name));
    }
}