using UnityEngine;
using TMPro;
using Battle;

public class BattleCommencement : IUiState
{
    private readonly float _period = 2;
    private float _elapsed = 0;
    private TMP_Text _commenceText;
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
            _executor = new BattleCommenceAnimationExecutor(battleProperties);
            _animatorObject = battleProperties.uiObjects.transform.Find("AnimatorObject").GetComponent<AnimatorObject>();
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