using Battle;
using Battle.Weapons;
using Battle.Statuses;
using System;
using Cutscene.Actions;

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

    public static AnimationExecutor Get(Cutscene.Action action, BattleProperties battleProperties)
    {
        if (action.GetType() == typeof(Talk)) return new TalkAnimationExecutor((Talk)action, battleProperties);

        if (action.GetType() == typeof(CameraMove)) return new CameraMoveAnimationExecutor((CameraMove)action, battleProperties);

        if (action.GetType() == typeof(CameraPan)) return new CameraPanAnimationExecutor((CameraPan)action, battleProperties);

        throw new InvalidOperationException($"AnimationExecutor for {action.GetType()} has not been implemented.");
    }
}