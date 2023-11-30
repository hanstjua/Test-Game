using Battle;
using UnityEngine;

public class BattleCommenceAnimationExecutor : AnimationExecutor
{
    public BattleCommenceAnimationExecutor(BattleProperties battleProperties) : base(battleProperties, new ActionOutcome[] {}) {}

    public override bool Execute(Animator animator)
    {
        var parameters = animator.GetComponent<BattleCommenceAnimationParameters>();
        parameters.battleProperties = BattleProperties;

        animator.Play("BattleCommence");
        animator.SetBool("play", true);

        return true;
    }

    public override bool IsAnimating(Animator animator)
    {
       return animator.GetBool("play");
    }
}