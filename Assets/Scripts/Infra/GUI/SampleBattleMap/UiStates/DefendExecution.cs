using Battle;
using Battle.Services.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DefendExecution : IUiState
{
    private ActionOutcome _outcome;
    private bool _hasInit = false;
    private IUiState _nextState;
    private AnimatorObject _animatorObject;
    private DefendAnimationExecutor _executor;

    public DefendExecution(ActionOutcome outcome, IUiState nextState)
    {
        _outcome = outcome;
        _nextState = nextState;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit)
        {
            _executor = new DefendAnimationExecutor(battleProperties, new ActionOutcome[] {_outcome});
            _animatorObject = battleProperties.uiObjects.transform.Find("AnimatorObject").GetComponent<AnimatorObject>();
            _animatorObject.Animate(_executor);

            _hasInit = true;
        }  

        if (!_animatorObject.IsAnimating(_executor))  // animation complete
        {
            return new AddStatusExecution((AddStatus) _outcome.Effects[0], _nextState);
        }
        else
        {
            return this;
        }
    }
}