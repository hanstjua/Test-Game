using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleCommence : StateMachineBehaviour
{
    private BattleCommenceAnimationParameters _parameters;
    private RawImage _rawImage;
    private TMP_Text _text;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _parameters = animator.GetComponent<BattleCommenceAnimationParameters>();
        _rawImage = _parameters.battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage").GetComponent<RawImage>();
        _text = _parameters.battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/CommenceText").GetComponent<TMP_Text>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var canvasAlpha = _parameters.canvasAlphaScaler;
        _rawImage.color = new Color(canvasAlpha, canvasAlpha, canvasAlpha, 1);

        _text.GetComponent<CanvasGroup>().alpha = _parameters.textAlphaScaler;

        animator.SetBool("play", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _rawImage.color = new Color(1, 1, 1, 1);

        _text.alpha = 0;

        animator.SetBool("play", false);
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
