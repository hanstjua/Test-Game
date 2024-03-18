using Battle;
using Battle.Statuses;
using UnityEngine;
using TMPro;

public class AddStatusAnimationExecutor : AnimationExecutor
{
    private const float DURATION = 1f;
    private const float HEIGHT_INCREASE = 1.5f;
    private const float WORLD_HEIGHT_OFFSET = 1.5f;

    AddStatus _effect;

    public AddStatusAnimationExecutor(AddStatus effect, BattleProperties battleProperties) : base(battleProperties, null)
    {
        _effect = effect;
    }

    public override bool Execute()
    {
        IsAnimating = true;

        var text = BattleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/DamageText").GetComponent<TMP_Text>();
        text.text = _effect.Status.Name;

        // put text above target
        var target = BattleProperties.characters[_effect.On];
        text.transform.position = target.transform.position + new Vector3(0, WORLD_HEIGHT_OFFSET, 0);

        var textCanvasGroup = text.GetComponent<CanvasGroup>();
        textCanvasGroup.alpha = 1;

        LeanTween.sequence()
        .insert(LeanTween.value(0,1, DURATION * 0.4f))
        .insert(LeanTween.move(text.gameObject, text.transform.position + new Vector3(0, HEIGHT_INCREASE, 0), DURATION * 0.5f))
        .insert(LeanTween.value(1, 0, DURATION * 0.3f).setOnUpdate((float val) => textCanvasGroup.alpha = val))
        .append(0.5f)
        .append(() => IsAnimating = false);

        return true;
    }
}