using Battle;
using Battle.Services.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AttackExecution : IUiState
{
    private readonly ActionOutcome _outcome;
    private bool _hasInit = false;
    private readonly IUiState _nextState;
    private AnimatorObject _animatorObject;
    private AttackAnimationExecutor _executor;

    public AttackExecution(ActionOutcome outcome, IUiState nextState)
    {
        _outcome = outcome;
        _nextState = nextState;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit)
        {
            _executor = new AttackAnimationExecutor(battleProperties, new ActionOutcome[] {_outcome});
            _animatorObject = battleProperties.uiObjects.transform.Find("AnimatorObject").GetComponent<AnimatorObject>();
            _animatorObject.Animate(_executor);

            _hasInit = true;
        }  

        if (!_animatorObject.IsAnimating(_executor))  // animation complete
        {
            return _nextState;
        }
        else
        {
            return this;
        }
    }
}