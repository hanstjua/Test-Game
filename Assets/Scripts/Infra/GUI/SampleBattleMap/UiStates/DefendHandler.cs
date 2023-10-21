using System;
using Battle;
using Battle.Services;
using UnityEngine;
using Battle.Services.Actions;

public class DefendHandler : ActionHandler
{
    private BattleProperties _battleProperties;
    private IUiState _onCancelState;
    private IUiState _onProceedState;

    public override Battle.Action Service => new Defend();

    public override IUiState Handle(BattleProperties battleProperties, IUiState onCancelState, IUiState onProceedState)
    {
        _battleProperties = battleProperties;
        _onCancelState = onCancelState;
        _onProceedState = onProceedState;
        
        return new SelectActionTarget(this);
    }

    public override bool ValidateTarget(Agent agent)
    {
        var battle = _battleProperties.unitOfWork.BattleRepository.Get(_battleProperties.battleId);

        return agent.Id() as AgentId == battle.ActiveAgent;  // can only select self
    }

    public override IUiState ExecuteAction(Agent target)
    {
        var battle = _battleProperties.unitOfWork.BattleRepository.Get(_battleProperties.battleId);
        var actor = _battleProperties.unitOfWork.AgentRepository.Get(battle.ActiveAgent);

        var outcomes = Service.Execute(actor, new Agent[] {actor}, battle, _battleProperties.unitOfWork);

        return new DefendExecution(outcomes[0].Effects[0], _onProceedState);
    }

    public override IUiState CancelAction()
    {
        return _onCancelState;
    }
}