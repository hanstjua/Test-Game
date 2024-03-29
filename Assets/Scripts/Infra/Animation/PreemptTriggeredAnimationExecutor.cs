using Battle;
using TMPro;
using UnityEngine;

public class PreemptTriggeredAnimationExecutor : AnimationExecutor
{
    private const float DURATION = 1f;
    private const float HEIGHT_INCREASE = 1.5f;
    private const float WORLD_HEIGHT_OFFSET = 1.5f;

    public PreemptTriggeredAnimationExecutor(BattleProperties battleProperties, ActionOutcome[] outcomes) : base(battleProperties, outcomes)
    {
    }

    public override bool Execute()
    {
        IsAnimating = true;

        var targetId = Outcomes[0].By;
        var effect = (PreemptTriggered) Outcomes[0].Effects[0];
        var target = BattleProperties.characters[targetId];

        var uiObjects = BattleProperties.uiObjects.transform;
        
        var popup = BattleProperties
        .unitOfWork
        .BattleRepository
        .Get(BattleProperties.battleId)
        .EnemyIds
        .Contains(targetId) ?
        uiObjects.Find("CameraCanvas/RawImage/EnemyActionPopup").GetComponent<ActionPopup>() :
        uiObjects.Find("CameraCanvas/RawImage/PlayerActionPopup").GetComponent<ActionPopup>();

        popup.SetPreempt(effect.Action.ToString());
        popup.Show();

        var camera = Camera.main.transform.GetComponent<CameraControl>();
        camera.FocusAt(target.transform.position);

        LeanTween.sequence()
        .append(1f)
        .append(() => {
            popup.Hide();
            IsAnimating = false;
        });

        return true;
    }
}