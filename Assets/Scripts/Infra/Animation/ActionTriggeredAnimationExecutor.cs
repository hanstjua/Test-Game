using Battle;
using TMPro;
using UnityEngine;

public class ActionTriggeredAnimationExecutor : AnimationExecutor
{
    private const float DURATION = 1f;
    private const float HEIGHT_INCREASE = 1.5f;
    private const float WORLD_HEIGHT_OFFSET = 1.5f;
    private static Pointer _pointer;
    private string _action;
    private Agent _actor;

    public ActionTriggeredAnimationExecutor(BattleProperties battleProperties, Agent actor, string action) : base(battleProperties, new ActionOutcome[] {})
    {
        _action = action;
        _actor = actor;
    }

    public override bool Execute()
    {
        IsAnimating = true;

        var targetId = _actor.Id() as AgentId;
        var target = BattleProperties.characters[targetId];

        _pointer = Pointer.CreateActorPointer(BattleProperties.map);
        _pointer.Position = BattleProperties.unitOfWork.AgentRepository.Get(targetId).Position;
        Debug.Log($"{_pointer.Position}");

        var uiObjects = BattleProperties.uiObjects.transform;
        
        var popup = BattleProperties
        .unitOfWork
        .BattleRepository
        .Get(BattleProperties.battleId)
        .EnemyIds
        .Contains(targetId) ?
        uiObjects.Find("CameraCanvas/RawImage/EnemyActionPopup").GetComponent<ActionPopup>() :
        uiObjects.Find("CameraCanvas/RawImage/PlayerActionPopup").GetComponent<ActionPopup>();

        popup.SetNormal(_action);
        popup.Show();

        var camera = Camera.main.transform.GetComponent<CameraControl>();
        camera.FocusAt(target.transform.position);

        LeanTween.sequence()
        .append(1f)
        .append(() => {
            popup.Hide();
            IsAnimating = false;
            _pointer.Destroy();
        });

        return true;
    }
}