using Battle;

public class InflictDamageExecution : IUiState
{
    private readonly HpDamage _effect;
    private bool _hasInit = false;
    private readonly IUiState _nextState;
    private AnimatorObject _animatorObject;
    private AnimationExecutor _executor;
    private readonly ActionOutcome _outcome;

    public InflictDamageExecution(HpDamage effect, IUiState nextState)
    {
        _effect = effect;
        _nextState = nextState;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit)
        {
            _executor = new InflictDamageAnimationExecutor(_effect, battleProperties);
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