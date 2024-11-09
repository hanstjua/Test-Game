using Battle;
using TMPro;
using UnityEngine;

public class AttackAnimationExecutor : AnimationExecutor
{
    private const float DURATION = 1f;
    private const float HEIGHT_INCREASE = 1.5f;
    private const float WORLD_HEIGHT_OFFSET = 1.5f;

    public AttackAnimationExecutor(BattleProperties battleProperties, ActionOutcome[] outcomes) : base(battleProperties, outcomes)
    {
    }

    public override bool Execute()
    {
        IsAnimating = true;
        
        var text = BattleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/DamageText").GetComponent<TMP_Text>();
        text.text = "0";

        // put text above target
        var hpDamage = Outcomes[0].Effects[0] as HpDamage;
        var target = BattleProperties.characters[hpDamage.On];

        Camera.main.GetComponent<CameraControl>().FocusAt(target.transform.position);

        text.transform.position = target.transform.position + new Vector3(0, WORLD_HEIGHT_OFFSET, 0);

        var textCanvasGroup = text.GetComponent<CanvasGroup>();

        LeanTween.sequence()
        .append(0.5f)
        .append(
            LeanTween.value(0, 1, DURATION * 0.1f)
            .setOnUpdate((float val) => {
                text.text = $"{(int)(hpDamage.Amount * val)}";
                text.transform.position = target.transform.position + new Vector3(0, WORLD_HEIGHT_OFFSET, 0);
                textCanvasGroup.alpha = 1;
            })
        )
        .insert(LeanTween.value(0, 1, DURATION * 0.4f))
        .insert(LeanTween.value(0, 1, DURATION * 0.5f).setOnUpdate((float val) => text.transform.position = target.transform.position + new Vector3(0, WORLD_HEIGHT_OFFSET + HEIGHT_INCREASE * val, 0)))
        .insert(LeanTween.value(1, 0, DURATION * 0.3f).setOnUpdate((float val) => textCanvasGroup.alpha = val))
        .append(0.5f)
        .append(() => IsAnimating = false);

        return true;
    }
}