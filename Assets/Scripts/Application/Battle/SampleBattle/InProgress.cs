using System;
using System.Linq;
using Battle;

namespace Battle.SampleBattle
{
    public class InProgress : Phase
    {
        private UnitOfWork _unitOfWork;
        public InProgress(UnitOfWork unitOfWork) : base("SampleBattle.InProgress")
        {
            _unitOfWork = unitOfWork;
        }

        public override Phase Transition(Battle battle)
        {
            if (battle.PlayerIds.Select(id => _unitOfWork.AgentRepository.Get(id)).All(p => !p.IsAlive()))
            {
                return new Defeat(_unitOfWork);
            }
            else if (battle.EnemyIds.Select(id => _unitOfWork.AgentRepository.Get(id)).All(e => !e.IsAlive()))
            {
                return new Victory(_unitOfWork);
            }
            else
            {
                using (_unitOfWork)
                {
                    // reset current active agent's turn gauge
                    var currentAgent = _unitOfWork.AgentRepository.Get(battle.ActiveAgent);
                    currentAgent.ResetTurnGauge();
                    _unitOfWork.AgentRepository.Update(battle.ActiveAgent, currentAgent);

                    // find next active agent
                    var agents = _unitOfWork.AgentRepository.GetAll().OrderByDescending(a => a.TurnGauge).ToArray();

                    var activeAgent = agents.FirstOrDefault(a => a.TurnGauge >= 100);
                    while (activeAgent == null)
                    {
                        foreach(var a in agents)
                        {
                            a.RaiseTurnGauge();
                        }

                        activeAgent = agents.FirstOrDefault(a => a.TurnGauge >= 100);
                    }

                    _unitOfWork.BattleRepository.Update(battle.Id() as BattleId, battle.NextTurn(activeAgent.Id() as AgentId));
                }

                return this;
            }
        }
    }
}