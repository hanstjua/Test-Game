using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battle.SampleBattle
{
	public class Preparing : Phase
	{
		private UnitOfWork _unitOfWork;

		public Preparing(UnitOfWork unitOfWork) : base("SampleBattle.Preparing")
        {
            _unitOfWork = unitOfWork;
        }

		public override Phase Transition(Battle battle)
		{
			using (_unitOfWork)
			{
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

			return new InProgress(_unitOfWork);
		}
	}
	
    public class Setup
    {
        private static System.Random _random = new System.Random();
        private UnitOfWork _unitOfWork;
        private BattleFieldId _fieldId;

        public Setup(
            UnitOfWork unitOfWork,
            BattleFieldId fieldId
        )
        {
            _unitOfWork = unitOfWork;
            _fieldId = fieldId;
        }

        public BattleId Execute()
        {
			BattleId battleId;
			using (_unitOfWork)
            {
				var battleField = _unitOfWork.BattleFieldRepository.Get(_fieldId);

				var players = GeneratePlayers(new List<string> {"James", "Jane"}, battleField.PlayerStartingPositions).Select(p => (AgentId)p.Id());

				var enemies = GenerateEnemies(battleField.Terrains).Select(e => (AgentId)e.Id());

				var phase = new Preparing(_unitOfWork);

				battleId = _unitOfWork.BattleRepository.Create(players.ToList(), enemies.ToList(), (BattleFieldId)battleField.Id(), phase);

				_unitOfWork.Save();
			}

            return battleId;
        }

        private List<Agent> GenerateEnemies(Terrain[][] terrains)
		{
			int enemiesCount = _random.Next(4, 50);

			var flatTerrains = terrains
			.Aggregate(new List<Terrain> {}, (acc, arr) => acc.Concat(arr).ToList())
			.Where(t => !t.IsStartingPosition && t.Traversable)
			.ToList();

			var shuffledTerrains = Shuffle(flatTerrains);

			return Enumerable.Range(0, enemiesCount)
			.Select(i => _unitOfWork.AgentRepository.CreateGenericEnemyByName("Goblin", shuffledTerrains[i].Position))
			.ToList();
		}

		private List<Agent> GeneratePlayers(List<string> characterNames, List<Position> startingPositions)
		{
			var randomizedPositions = Shuffle(startingPositions);
			var characters = Enumerable.Range(0, characterNames.Count())
			.Select(i => _unitOfWork.AgentRepository.CreateAgentByName(characterNames[i], startingPositions[i]))
			.ToList();

			return Enumerable.Range(0, characters.Count())
			.Select(i => characters[i].Move(startingPositions[i]))
			.ToList();
		}

        private List<T> Shuffle<T>(List<T> items)
		{
			var ret = new List<T>();
			var indices = Enumerable.Range(0, items.Count).ToList();
			for(int i=0; i<items.Count; i++)
			{
				var index = _random.Next(0, indices.Count - 1);
				ret.Add(items[index]);
				indices.RemoveAt(index);
			}

			return ret;
		}
    }
}