using Battle;
using Battle.Statuses;
using UnityEngine;

public class DefendAnimationExecutor : AnimationExecutor
{
    ActionOutcome[] _outcomes;

    public DefendAnimationExecutor(BattleProperties battleProperties, ActionOutcome[] outcomes) : base(battleProperties, outcomes)
    {
        _outcomes = outcomes;
    }

    public override bool Execute(Animator animator)
    {
        var parameters = animator.GetComponent<DefendAnimationParameters>();
        parameters.battleProperties = BattleProperties;
        parameters.on = _outcomes[0].On[0];

        animator.Play("Defend");
        animator.SetBool("play", true);

        return true;
    }

    public override bool IsAnimating(Animator animator)
    {
       return animator.GetBool("play");
    }
}