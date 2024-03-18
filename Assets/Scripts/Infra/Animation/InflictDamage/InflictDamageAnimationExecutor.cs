using Battle;
using UnityEngine;

public class InflictDamageAnimationExecutor : AnimationExecutor
{
    public InflictDamageAnimationExecutor(HpDamage effect, BattleProperties battleProperties) : base(battleProperties, null)
    {
    }

    public override bool Execute()
    {
        // var parameters = animator.GetComponent<InflictDamageAnimationParameters>();
        // parameters.hpDamage = Outcomes[0].Effects[0] as HpDamage;
        // parameters.battleProperties = BattleProperties;

        // animator.Play("InflictDamage.InflictDamage");
        // animator.SetBool("play", true);

        return true;
    }
}