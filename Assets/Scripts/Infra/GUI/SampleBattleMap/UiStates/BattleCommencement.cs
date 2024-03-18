using UnityEngine;

public class BattleCommencement : IUiState
{
    private bool _hasInit = false;
    private BattleCommenceAnimationExecutor _executor;
    private AnimatorObject _animatorObject;

    public BattleCommencement(GameObject uiObjects)
    {

    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit)
        {
            _animatorObject = battleProperties.uiObjects.transform.Find("AnimatorObject").GetComponent<AnimatorObject>();
            _executor = new BattleCommenceAnimationExecutor(battleProperties, _animatorObject.GetComponent<Animator>());
            _animatorObject.Animate(_executor);

            _hasInit = true;
        }

        if (!_animatorObject.IsAnimating(_executor))  // animation complete
        {
            return new TransitionBattlePhase();
        }
        else
        {
            return this;
        }
    }
}