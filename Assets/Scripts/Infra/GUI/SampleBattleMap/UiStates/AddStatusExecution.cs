using Battle;
using Battle.Statuses;
using System.Linq;

public class AddStatusExecution : IUiState
{
    private readonly AddStatus _effect;
    private bool _hasInit = false;
    private readonly IUiState _nextState;
    private AnimatorObject _animatorObject;
    private AnimationExecutor _executor;

    public AddStatusExecution(AddStatus effect, IUiState nextState)
    {
        _effect = effect;
        _nextState = nextState;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit)
        {
            _executor = new AddStatusAnimationExecutor(_effect, battleProperties);
            _animatorObject = battleProperties.uiObjects.transform.Find("AnimatorObject").GetComponent<AnimatorObject>();
            _animatorObject.Animate(_executor);

            _hasInit = true;
        }  

        if (!_animatorObject.IsAnimating(_executor))  // animation complete
        {
            return _nextState;
        }
        else
        {
            return this;
        }
    }
}