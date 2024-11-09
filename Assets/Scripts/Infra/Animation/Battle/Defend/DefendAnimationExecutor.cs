using Battle;
using TMPro;
using UnityEngine;

public class DefendAnimationExecutor : AnimationExecutor
{
    private const float DURATION = 1f;
    private const float HEIGHT_INCREASE = 1.5f;
    private const float WORLD_HEIGHT_OFFSET = 1.5f;

    public DefendAnimationExecutor(BattleProperties battleProperties, ActionOutcome[] outcomes) : base(battleProperties, outcomes)
    {
    }

    public override bool Execute()
    {
        IsAnimating = true;

        var text = BattleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/DamageText").GetComponent<TMP_Text>();
        text.text = "Defending!";

        // put text above target
        var target = BattleProperties.characters[Outcomes[0].On[0]];
        text.transform.position = target.transform.position + new Vector3(0, WORLD_HEIGHT_OFFSET, 0);

        var textCanvasGroup = text.GetComponent<CanvasGroup>();
        textCanvasGroup.alpha = 1;

        LeanTween.sequence()
        .insert(LeanTween.value(0,1, DURATION * 0.4f))
        .insert(LeanTween.move(text.gameObject, text.transform.position + new Vector3(0, HEIGHT_INCREASE, 0), DURATION * 0.5f))
        .insert(LeanTween.value(1, 0, DURATION * 0.3f).setOnUpdate((float val) => textCanvasGroup.alpha = val))
        .append(() => IsAnimating = false);

        return true;
    }
}