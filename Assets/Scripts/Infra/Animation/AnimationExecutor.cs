using System;
using Battle;
using UnityEngine;

public abstract class AnimationExecutor
{
    public BattleProperties BattleProperties { get; private set; }
    public ActionOutcome[] Outcomes { get; private set; }
    private bool _isAnimating;
    private AnimationExecutor _next = null;
    
    public AnimationExecutor(BattleProperties battleProperties, ActionOutcome[] outcomes)
    {
        BattleProperties = battleProperties;
        Outcomes = outcomes;
    }

    public bool IsAnimating
    {
        get
        {
            return _next != null ? _isAnimating || _next.IsAnimating : _isAnimating;
        }

        protected set
        {
            _isAnimating = value;

            if (_next != null && !value) _next.Execute();
        }
    }

    public abstract bool Execute();

    public AnimationExecutor Then(AnimationExecutor executor)
    {
        _next = executor;
        return executor;
    }
}