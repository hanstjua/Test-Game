using System;
using Battle;
using Battle.Actions;
using UnityEngine;


public class AttackHandler : IActionHandler
{
    private BattleProperties _battleProperties;

    public IUiState Handle(BattleProperties battleProperties, IUiState currentState)
    {
        _battleProperties = battleProperties;
        
        return new SelectActionTarget(ValidateTarget, HandleAction, () => currentState);
    }

    public bool ValidateTarget(Agent agent)
    {
        var battle = _battleProperties.unitOfWork.BattleRepository.Get(_battleProperties.battleId);

        return agent.Id() as AgentId != battle.ActiveAgent;  // cannot attack self
    }

    public IUiState HandleAction(Agent target)
    {
        var battle = _battleProperties.unitOfWork.BattleRepository.Get(_battleProperties.battleId);
        var targetId = target.Id() as AgentId;
        var outcomes = new AttackUseCase(_battleProperties.unitOfWork).Execute(battle.ActiveAgent.Uuid, targetId.Uuid);

        var unitOfWork = _battleProperties.unitOfWork;
        using (unitOfWork)
        {
            foreach(var outcome in outcomes)
            {
                var agent = unitOfWork.AgentRepository.Get(outcome.On);

                if (outcome.HpDamage > 0) agent = agent.ReduceHp((int) outcome.HpDamage);
                else if (outcome.HpDamage < 0) agent = agent.RestoreHp((int) outcome.HpDamage);

                if (outcome.MpDamage > 0) agent = agent.ReduceMp((int) outcome.MpDamage);
                else if (outcome.MpDamage < 0) agent = agent.RestoreMp((int) outcome.MpDamage);

                if (outcome.AddStatuses != null)
                {
                    foreach(var status in outcome.AddStatuses)
                    {
                        agent = agent.AddStatus(status);
                    }
                }

                if (outcome.RemoveStatuses != null)
                {
                    foreach(var status in outcome.AddStatuses)
                    {
                        agent = agent.RemoveStatus(status);
                    }
                }

                unitOfWork.AgentRepository.Update(outcome.On, agent);
            }
        }

        return new AttackExecution(outcomes);
    }
}