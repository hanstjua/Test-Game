using Battle;
using Cutscene.Actions;
using System;
using System.Collections;
using UnityEngine;

class CameraMoveAnimationExecutor : AnimationExecutor
{
    private readonly CameraMove _cameraMove;

    public CameraMoveAnimationExecutor(CameraMove cameraMove, BattleProperties battleProperties) : base(battleProperties, new ActionOutcome[] {})
    {
        _cameraMove = cameraMove;
    }

    public override bool Execute()
    {
        var camera = Camera.main.GetComponent<CameraControl>();
        var coord = _cameraMove.By.Coord;
        var pos = BattleProperties.map.ToUIPosition(new(coord[0], coord[1], 0));

        IsAnimating = true;

        LeanTween.sequence()
        .insert(LeanTween.move(camera.gameObject, new Vector3(pos.x, 0, pos.z) + camera.transform.position, (float)_cameraMove.Duration))
        .append(() => IsAnimating = false);

        return true;
    }
}