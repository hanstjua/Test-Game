using Battle;
using Battle.Services.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ActionTriggeredExecution : IUiState
{
    private readonly Agent _actor;
    private readonly string _action;
    private bool _hasInit = false;
    private readonly IUiState _nextState;
    private ActionTriggeredAnimationExecutor _executor;

    public ActionTriggeredExecution(Agent actor, string action, IUiState nextState)
    {
        _actor = actor;
        _action = action;
        _nextState = nextState;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit)
        {
            _executor = new ActionTriggeredAnimationExecutor(battleProperties, _actor, _action);
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