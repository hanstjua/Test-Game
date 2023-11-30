using Battle;
using UnityEngine;

public class AttackAnimationExecutor : AnimationExecutor
{
    public AttackAnimationExecutor(BattleProperties battleProperties, ActionOutcome[] outcomes) : base(battleProperties, outcomes)
    {
    }

    public override bool Execute(Animator animator)
    {
        var parameters = animator.GetComponent<AttackAnimationParameters>();
        parameters.hpDamage = Outcomes[0].Effects[0] as HpDamage;
        parameters.battleProperties = BattleProperties;

        animator.Play("Attack.AttackAnimation");
        animator.SetBool("play", true);

        return true;
    }

    public override bool IsAnimating(Animator animator)
    {
       return animator.GetBool("play");
    }
}