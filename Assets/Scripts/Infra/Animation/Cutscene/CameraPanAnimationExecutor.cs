using Battle;
using Cutscene.Actions;
using System;
using System.Collections;
using UnityEngine;

class CameraPanAnimationExecutor : AnimationExecutor
{
    private readonly CameraPan _cameraPan;

    public CameraPanAnimationExecutor(CameraPan cameraPan, BattleProperties battleProperties) : base(battleProperties, new ActionOutcome[] {})
    {
        _cameraPan = cameraPan;
    }

    public override bool Execute()
    {
        var camera = Camera.main.GetComponent<CameraControl>();

        IsAnimating = true;

        camera.Rotate(_cameraPan.Degrees, _cameraPan.Duration);
        camera.StartCoroutine(MonitorAnimation(camera.gameObject));

        return true;
    }

    IEnumerator MonitorAnimation(GameObject camera)
    {
        while (LeanTween.isTweening(camera))
        {
            yield return null;
        }

        IsAnimating = false;
    }
}