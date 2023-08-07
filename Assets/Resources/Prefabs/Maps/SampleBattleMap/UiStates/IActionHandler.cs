using System;


public interface IActionHandler
{
    public IUiState Handle(BattleProperties battleProperties, IUiState currentState);
}