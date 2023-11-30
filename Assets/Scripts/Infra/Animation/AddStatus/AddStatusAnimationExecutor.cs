using Battle;
using Battle.Statuses;
using UnityEngine;

public class AddStatusAnimationExecutor : AnimationExecutor
{
    AddStatus _effect;

    public AddStatusAnimationExecutor(AddStatus effect, BattleProperties battleProperties) : base(battleProperties, null)
    {
        _effect = effect;
    }

    public override bool Execute(Animator animator)
    {
        var parameters = animator.GetComponent<AddStatusAnimationParameters>();
        parameters.effect = _effect;
        parameters.battleProperties = BattleProperties;

        animator.Play("AddStatus");
        animator.SetBool("play", true);

        return true;
    }

    public override bool IsAnimating(Animator animator)
    {
       return animator.GetBool("play");
    }
}