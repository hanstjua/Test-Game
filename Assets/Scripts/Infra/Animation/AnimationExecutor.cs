using Battle;
using UnityEngine;

public abstract class AnimationExecutor
{
    public BattleProperties BattleProperties { get; private set; }
    public ActionOutcome[] Outcomes { get; private set; }
    
    public AnimationExecutor(BattleProperties battleProperties, ActionOutcome[] outcomes)
    {
        BattleProperties = battleProperties;
        Outcomes = outcomes;
    }

    public abstract bool Execute(Animator animator);
    public abstract bool IsAnimating(Animator animator);
}