using System.Linq;
using Battle;

public class TransitionBattlePhase : IUiState
{
    private ActionOutcome[] _statusOutcomes;
    private bool _hasInit = false;
    private Agent _activeAgent;

    private void Init(BattleProperties battleProperties)
    {
        using (var unitOfWork = battleProperties.unitOfWork)
        {
            var battle = unitOfWork.BattleRepository.Get(battleProperties.battleId);
            unitOfWork.BattleRepository.Update(battleProperties.battleId, battle.NextPhase());
            _activeAgent = unitOfWork.AgentRepository.Get(battle.ActiveAgent);

            unitOfWork.Save();
        }

        var statuses = _activeAgent.Statuses;

        if (statuses.Count > 0)
        {
            var outcomes = statuses
            .Select(s => s.Apply(_activeAgent, battleProperties.unitOfWork.BattleRepository.Get(battleProperties.battleId), battleProperties.unitOfWork))
            .Aggregate((acc, next) => acc.Concat(next).ToArray());

            _statusOutcomes = outcomes.Count() > 0 ? outcomes : null;
        }

        _hasInit = true;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit) Init(battleProperties);

        IUiState nextState;

        var battle = battleProperties.unitOfWork.BattleRepository.Get(battleProperties.battleId);
        if (battle.Phase.Equals(new Victory(battleProperties.unitOfWork)))
        {
            nextState = new VictoryScreen();
        }
        // handle when phase is victory/game over 
        else nextState = new CharacterTurn(_activeAgent.Id() as AgentId, false, false, _activeAgent.Position, _activeAgent.Direction);

        return _statusOutcomes == null ? nextState : new ActionExecution(_statusOutcomes, nextState);
    }
}