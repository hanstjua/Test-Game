using System;
using Battle;
using Battle.Services;
using UnityEngine;
using Battle.Services.Actions;
using System.Linq;

public class AttackHandler : ActionHandler
{
    private BattleProperties _battleProperties;
    private IUiState _onCancelState;
    private IUiState _onProceedState;

    public override Battle.Action Service => new Attack();

    public override IUiState Handle(BattleProperties battleProperties, IUiState onCancelState, IUiState onProceedState)
    {
        _battleProperties = battleProperties;
        _onCancelState = onCancelState;
        _onProceedState = onProceedState;
        
        return new SelectActionTarget(this);
    }

    public override bool ValidateTarget(Agent target)
    {
        var battle = _battleProperties.unitOfWork.BattleRepository.Get(_battleProperties.battleId);
        var actor = _battleProperties.unitOfWork.AgentRepository.Get(battle.ActiveAgent);
        var relativePosition = actor.Position.RelativeTo(target.Position);

        var aoe = Service.AreaOfEffect;

        Func<Position, bool> isWithinAreaOfEffect = (Position p) => 
        relativePosition.X == p.X &&
        relativePosition.Y == p.Y && 
        Math.Abs(relativePosition.Z) <= aoe.Height;

        var isNotSelf = target.Id() as AgentId != battle.ActiveAgent;
        var isInRange = aoe.RelativePositions.Any(isWithinAreaOfEffect);

        return isNotSelf && isInRange;  // cannot attack self
    }

    public override IUiState ExecuteAction(Agent target)
    {
        var battle = _battleProperties.unitOfWork.BattleRepository.Get(_battleProperties.battleId);
        var actor = _battleProperties.unitOfWork.AgentRepository.Get(battle.ActiveAgent);

        var outcomes = Service.Execute(actor, new Agent[] {target}, battle, _battleProperties.unitOfWork);

        _battleProperties.battleEvents.actionExecuted.Invoke(outcomes);

        return new AttackExecution(outcomes, _onProceedState);
    }

    public override IUiState CancelAction()
    {
        return _onCancelState;
    }
}