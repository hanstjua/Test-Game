using Battle;
using Battle.Services.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PreemptTriggeredExecution : IUiState
{
    private readonly ActionOutcome _outcome;
    private bool _hasInit = false;
    private readonly IUiState _nextState;
    private PreemptTriggeredAnimationExecutor _executor;

    public PreemptTriggeredExecution(ActionOutcome outcome, IUiState nextState)
    {
        _outcome = outcome;
        _nextState = nextState;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit)
        {
            _executor = new PreemptTriggeredAnimationExecutor(battleProperties, new ActionOutcome[] {_outcome});
            _executor.Execute();

            _hasInit = true;
        }  

        if (!_executor.IsAnimating)  // animation complete
        {
            return _nextState;
        }
        else
        {
            return this;
        }
    }
}