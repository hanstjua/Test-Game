using Battle;
using Battle.Statuses;
using System.Linq;

public class LongswordExecution : IUiState
{
    private readonly ActionOutcome _outcome;
    private readonly IUiState _nextState;


    public LongswordExecution(ActionOutcome outcome, IUiState nextState)
    {
        _outcome = outcome;
        _nextState = nextState;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        var addPoison = new AddStatusExecution((AddStatus) _outcome.Effects[1], _nextState);
        var addGuard = new AddStatusExecution((AddStatus) _outcome.Effects[0], addPoison);

        return addGuard;
    }
}