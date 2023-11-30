using Battle;

public class LeatherArmourExecution : IUiState
{
    private readonly ActionOutcome _outcome;
    private readonly IUiState _nextState;


    public LeatherArmourExecution(ActionOutcome outcome, IUiState nextState)
    {
        _outcome = outcome;
        _nextState = nextState;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        return _outcome.Effects[0] is AddStatus ? 
        new AddStatusExecution((AddStatus) _outcome.Effects[0], _nextState) :
        new InflictDamageExecution((HpDamage) _outcome.Effects[0], _nextState);
    }
}