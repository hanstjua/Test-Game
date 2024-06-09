using System;
using Inventory;
using Battle;

namespace Common
{
    public class UnitOfWork : IDisposable
    {
        public UnitOfWork(
            IAgentRepository agentRepository,
            IBattleRepository battleRepository,
            IBattleFieldRepository battleFieldRepository,
            IInventoryRepository inventoryRepository
        )
        {
            AgentRepository = agentRepository;
            BattleRepository = battleRepository;
            BattleFieldRepository = battleFieldRepository;
            InventoryRepository = inventoryRepository;
        }

        public IAgentRepository AgentRepository { get; private set; }
        public IBattleRepository BattleRepository { get; private set; }
        public IBattleFieldRepository BattleFieldRepository { get; private set; }
        public IInventoryRepository InventoryRepository { get; private set; }

        public UnitOfWork Save()
        {
            AgentRepository.Save();
            BattleRepository.Save();
            BattleFieldRepository.Save();
            InventoryRepository.Save();

            return this;
        }

        public void Dispose()
        {
            AgentRepository.Reload();
            BattleRepository.Reload();
            BattleFieldRepository.Reload();
            InventoryRepository.Reload();
        }
    }
}