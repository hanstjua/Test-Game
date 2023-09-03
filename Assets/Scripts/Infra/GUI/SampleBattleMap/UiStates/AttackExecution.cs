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
    private readonly ActionOutcome[] _outcomes;
    private bool _hasInit = false;
    private readonly IUiState _nextState;
    private AnimatorObject _animatorObject;
    private AttackAnimationExecutor _executor;

    public AttackExecution(ActionOutcome[] outcomes, IUiState nextState)
    {
        _outcomes = outcomes;
        _nextState = nextState;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit)
        {
            _executor = new AttackAnimationExecutor(battleProperties, _outcomes);
            _animatorObject = battleProperties.uiObjects.transform.Find("AnimatorObject").GetComponent<AnimatorObject>();
            _animatorObject.Animate(_executor);

            _hasInit = true;
        }  

        if (!_animatorObject.IsAnimating(_executor))  // animation complete
        {
            if (_outcomes.Count() == 1)
            {
                return _nextState;
            }
            else            
            {
                return new AttackExecution(_outcomes.Skip(1).ToArray(), _nextState);
            }
        }
        else
        {
            return this;
        }
    }
}