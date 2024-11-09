using Battle;
using Battle.Statuses;
using UnityEngine;

public class RemoveStatusAnimationExecutor : AnimationExecutor
{
    RemoveStatus _effect;

    public RemoveStatusAnimationExecutor(RemoveStatus effect, BattleProperties battleProperties) : base(battleProperties, null)
    {
        _effect = effect;
    }

    public override bool Execute()
    {
        // var parameters = animator.GetComponent<RemoveStatusAnimationParameters>();
        // parameters.effect = _effect;
        // parameters.battleProperties = BattleProperties;

        // animator.Play("Status.RemoveStatus");
        // animator.SetBool("play", true);

        return true;
    }
}