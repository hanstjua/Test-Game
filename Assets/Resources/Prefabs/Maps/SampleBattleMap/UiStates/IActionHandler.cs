using System;
using Battle;


public interface IActionHandler
{
    public IUiState Handle(BattleProperties battleProperties, IUiState currentState);
    public bool ValidateTarget(Agent agent);
    public IUiState ExecuteAction(Agent target);
    public IUiState CancelAction();
}