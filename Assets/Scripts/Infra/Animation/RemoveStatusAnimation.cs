using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RemoveStatusAnimation : StateMachineBehaviour
{
    private TMP_Text _text;
    private RemoveStatusAnimationParameters _parameters;
    private CanvasGroup _textCanvasGroup;
    private Vector2 _renderTextureSize;
    private Vector2 _screenSize;
    private Camera _mainCamera;

    private const int HEIGHT_INCREASE = 80;
    private const float WORLD_HEIGHT_OFFSET = 1f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _parameters = animator.GetComponent<RemoveStatusAnimationParameters>();
        _text = _parameters.battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/StatusAnim").GetComponent<TMP_Text>();
        _text.text = "0";

        _textCanvasGroup = _text.GetComponent<CanvasGroup>();

        // put text above target
        _mainCamera = Camera.main;
        var targetTexture = _mainCamera.targetTexture;
        _renderTextureSize = new Vector2(targetTexture.width, targetTexture.height);
        _screenSize = new Vector2(Screen.width, Screen.height);

        var target = _parameters.battleProperties.characters[_parameters.effect.On];
        var targetScreenPosition = _mainCamera.WorldToScreenPoint(target.transform.position + new Vector3(0, WORLD_HEIGHT_OFFSET, 0));

        _text.transform.position = new Vector3(
            targetScreenPosition.x * (float)_screenSize.x / (float)_renderTextureSize.x,
            targetScreenPosition.y * (float)_screenSize.y / (float)_renderTextureSize.y,
            0
        );
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callback
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _text.text = _parameters.effect.Name;
        _textCanvasGroup.alpha = _parameters.textAlphaScaler;
        animator.SetBool("play", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.SetBool("play", false);
       _text.text = "0";
       _textCanvasGroup.alpha = 0;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
