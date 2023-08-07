using System;
using Battle;
using UnityEngine;


public class TransitionBattlePhase : IUiState
{
    public IUiState Update(BattleProperties battleProperties)
    {
        Agent activeAgent;
        using (battleProperties.unitOfWork)
        {
            var battle = battleProperties.unitOfWork.BattleRepository.Get(battleProperties.battleId);
            battleProperties.unitOfWork.BattleRepository.Update(battleProperties.battleId, battle.NextPhase());
            activeAgent = battleProperties.unitOfWork.AgentRepository.Get(battle.ActiveAgent);
        }

        return new CharacterTurn(activeAgent);
    }
}