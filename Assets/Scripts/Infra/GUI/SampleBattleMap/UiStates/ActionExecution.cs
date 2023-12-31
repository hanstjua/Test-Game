using Battle;
using Battle.Services.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ActionExecution : IUiState
{
    private readonly ActionOutcome[] _outcomes;
    private bool _hasInit = false;
    private readonly IUiState _nextState;
    private AnimatorObject _animatorObject;
    private AnimationExecutor _executor;

    public ActionExecution(ActionOutcome[] outcomes, IUiState nextState)
    {
        _outcomes = outcomes;
        _nextState = nextState;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit)
        {
            _executor = AnimationExecutorFactory.Get(_outcomes[0].Cause, battleProperties, _outcomes.Take(1).ToArray());
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
                return new ActionExecution(_outcomes.Skip(1).ToArray(), _nextState);
            }
        }
        else
        {
            return this;
        }
    }
}