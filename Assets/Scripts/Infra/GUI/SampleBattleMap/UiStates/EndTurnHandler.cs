using Battle;

public class EndTurnHandler : ActionHandler
{
    public EndTurnHandler() : base(null)
    {
    }

    public override IUiState CancelAction()
    {
        throw new System.NotImplementedException();
    }

    public override IUiState ExecuteAction(Agent target)
    {
        throw new System.NotImplementedException();
    }

    public override IUiState Handle(BattleProperties battleProperties, IUiState onCancelState, IUiState onProceedState)
    {
        var battle = battleProperties.unitOfWork.BattleRepository.Get(battleProperties.battleId);
        var agent = battleProperties.unitOfWork.AgentRepository.Get(battle.ActiveAgent);
        
        return new SelectDirection(battle.ActiveAgent, agent.Position, _ => onCancelState);
    }

    public override bool ValidateTarget(Agent agent)
    {
        throw new System.NotImplementedException();
    }
}