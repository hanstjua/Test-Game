using System;
using System.Runtime.InteropServices;

namespace Battle
{
    public class UnitOfWork : IDisposable
    {
        public UnitOfWork(
            IAgentRepository agentRepository,
            IBattleRepository battleRepository,
            IBattleFieldRepository battleFieldRepository
        )
        {
            AgentRepository = agentRepository;
            BattleRepository = battleRepository;
            BattleFieldRepository = battleFieldRepository;
        }

        public IAgentRepository AgentRepository { get; private set; }
        public IBattleRepository BattleRepository { get; private set; }
        public IBattleFieldRepository BattleFieldRepository { get; private set; }

        public UnitOfWork Save()
        {
            AgentRepository.Save();
            BattleRepository.Save();
            BattleFieldRepository.Save();

            return this;
        }

        public void Dispose()
        {
            AgentRepository.Reload();
            BattleRepository.Reload();
            BattleFieldRepository.Reload();
        }
    }
}