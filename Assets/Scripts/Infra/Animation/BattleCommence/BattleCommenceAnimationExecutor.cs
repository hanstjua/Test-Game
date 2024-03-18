using Battle;
using UnityEngine;

public class BattleCommenceAnimationExecutor : AnimationExecutor
{
    Animator _animator;

    public BattleCommenceAnimationExecutor(BattleProperties battleProperties, Animator animator) : base(battleProperties, new ActionOutcome[] {})
    {
        _animator = animator;
    }

    public override bool Execute()
    {
        IsAnimating = true;
        var parameters = _animator.GetComponent<BattleCommenceAnimationParameters>();
        parameters.battleProperties = BattleProperties;
        parameters.executor = this;

        _animator.Play("BattleCommence");
        _animator.SetBool("play", true);

        return true;
    }

    public void UpdateAnimationStatus(bool isAnimating)
    {
        IsAnimating = isAnimating;
    }
}