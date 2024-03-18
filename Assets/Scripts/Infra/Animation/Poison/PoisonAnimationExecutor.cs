using Battle;
using Battle.Statuses;
using UnityEngine;

public class PoisonAnimationExecutor : AnimationExecutor
{
    ActionOutcome[] _outcomes;

    public PoisonAnimationExecutor(BattleProperties battleProperties, ActionOutcome[] outcomes) : base(battleProperties, outcomes)
    {
        _outcomes = outcomes;
    }

    public override bool Execute()
    {
        // var parameters = animator.GetComponent<PoisonAnimationParameters>();
        // parameters.hpDamage = _outcomes[0].Effects[0] as HpDamage;
        // parameters.battleProperties = BattleProperties;

        // animator.Play("Poison");
        // animator.SetBool("play", true);

        return true;
    }
}