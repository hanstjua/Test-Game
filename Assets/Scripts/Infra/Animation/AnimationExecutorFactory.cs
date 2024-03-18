using Battle;
using Battle.Common.Weapons;
using Battle.Statuses;
using System;

public class AnimationExecutorFactory
{
    public static AnimationExecutor Get(ActionType type, BattleProperties battleProperties, ActionOutcome[] outcomes)
    {
        if (type == ActionType.Attack) return new AttackAnimationExecutor(battleProperties, outcomes);

        if (type == WeaponType.Longsword) return new AttackAnimationExecutor(battleProperties, outcomes);

        if (type == ActionType.Defend) return new DefendAnimationExecutor(battleProperties, outcomes);
        
        if (type == StatusType.Poison) return new PoisonAnimationExecutor(battleProperties, outcomes);
        
        throw new InvalidOperationException(String.Format("No animation executor for {0}.", type.GetType().ToString()));
    }
}