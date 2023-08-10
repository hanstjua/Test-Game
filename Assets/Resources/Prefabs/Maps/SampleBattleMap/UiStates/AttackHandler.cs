using System;
using Battle;
using Battle.Services;
using Battle.Actions;
using UnityEngine;
using Battle.Services.Actions;

public class AttackHandler : IActionHandler
{
    private BattleProperties _battleProperties;
    private IUiState _prevState;

    public IUiState Handle(BattleProperties battleProperties, IUiState prevState)
    {
        _battleProperties = battleProperties;
        _prevState = prevState;
        
        return new SelectActionTarget(this);
    }

    public bool ValidateTarget(Agent agent)
    {
        var battle = _battleProperties.unitOfWork.BattleRepository.Get(_battleProperties.battleId);

        return agent.Id() as AgentId != battle.ActiveAgent;  // cannot attack self
    }

    public IUiState ExecuteAction(Agent target)
    {
        var battle = _battleProperties.unitOfWork.BattleRepository.Get(_battleProperties.battleId);
        var actor = _battleProperties.unitOfWork.AgentRepository.Get(battle.ActiveAgent);

        var outcomes = Attack.Execute(actor, target, battle, _battleProperties.unitOfWork);

        _battleProperties.battleEvents.actionExecuted.Invoke(outcomes);

        return new AttackExecution(outcomes);
    }

    public IUiState CancelAction()
    {
        return _prevState;
    }
}