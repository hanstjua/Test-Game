using Battle.Accessories;
using Battle.Armours;
using Battle.Footwears;
using Battle.Services.Arbella;
using Battle.Shieds;
using Battle.Weapons;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle.SampleBattle
{
	public class Preparing : Phase
	{
		private readonly UnitOfWork _unitOfWork;

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
				
				_unitOfWork.Save();
			}

			return new InProgress(_unitOfWork);
		}
	}
	
    public class Setup
    {
        private static readonly Random _random = new();
        private readonly UnitOfWork _unitOfWork;
        private readonly BattleFieldId _fieldId;

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
				// populate inventory
				var inventory = _unitOfWork
				.InventoryRepository
				.Get()
				.AddItem(Item.Potion, 10)
				.AddEquipment(new Longsword(), 3)
				.AddEquipment(new Buckler(), 3)
				.AddEquipment(new LeatherArmour(), 3)
				.AddEquipment(new IronBoots(), 3)
				.AddEquipment(new GoldNecklace(), 2)
				.AddEquipment(new SilverRing(), 2);

				_unitOfWork.InventoryRepository.Update(inventory);

				var battleField = _unitOfWork.BattleFieldRepository.Get(_fieldId);

				var players = GeneratePlayers(new List<string> {"James", "Jane"}, battleField.PlayerStartingPositions).Select(p => (AgentId)p.Id());

				var enemies = GenerateEnemies(battleField.Terrains).Select(e => (AgentId)e.Id());

				var phase = new Preparing(_unitOfWork);

				battleId = _unitOfWork.BattleRepository.Create(players.ToList(), enemies.ToList(), (BattleFieldId)battleField.Id(), phase);

				_unitOfWork.Save();
			}

            return battleId;
        }

		public Agent CreateAgentByName(string name, Position position, UnitOfWork unitOfWork)
		{
			var id = new AgentId(name.ToLower());
			var agent = new Agent(
				id,
				name,
				new Arbellum[] {new Physical(0, true)},
				new(100, 100, 100, 100, 100, 100, 100, 100, 3000, 1000),
				position,
				new(),
				2,
				null, null, null, null, null, null
			);

			using (unitOfWork)
			{
				unitOfWork.AgentRepository.Update(id, agent);
				unitOfWork.Save();
			}

			var service = new EquipService();
			service.Execute(id, new Longsword(), false, unitOfWork);
			service.Execute(id, new LeatherArmour(), unitOfWork);
			service.Execute(id, new IronBoots(), unitOfWork);
			service.Execute(id, new GoldNecklace(), true, unitOfWork);

			using (unitOfWork)
			{
				unitOfWork.AgentRepository.Update(id, agent);
				unitOfWork.Save();
			}

			return agent;
		}

		public Agent CreateGenericEnemyByName(string name, Position position, UnitOfWork unitOfWork)
		{
			var character = CharacterFactory.GetGeneric(name);

			var id = new AgentId(Guid.NewGuid().ToString());
			var agent = new Agent(
				id, 
				name, 
				character.Arbella, 
				character.Levels, 
				position, 
				character.Items, 
				character.Movements,
				null, null, null, null, null, null
			);

			using (unitOfWork)
			{
				unitOfWork.AgentRepository.Update(id, agent);
				unitOfWork.Save();
			}
			
			var service = new EquipService();
			service.Execute(id, character.LeftHand, false, unitOfWork);
			service.Execute(id, character.RightHand, true, unitOfWork);
			service.Execute(id, character.Armour, unitOfWork);
			service.Execute(id, character.Footwear, unitOfWork);
			service.Execute(id, character.Accessory1, true, unitOfWork);
			service.Execute(id, character.LeftHand, false, unitOfWork);

			using (unitOfWork)
			{
				unitOfWork.AgentRepository.Update(id, agent);
				unitOfWork.Save();
			}

			return agent;
		}

        private List<Agent> GenerateEnemies(Terrain[][] terrains)
		{
			int enemiesCount = _random.Next(1, 2);

			var flatTerrains = terrains
			.Aggregate(new List<Terrain> {}, (acc, arr) => acc.Concat(arr).ToList())
			.Where(t => !t.IsStartingPosition && t.Traversable)
			.ToList();

			var shuffledTerrains = Shuffle(flatTerrains);

			return Enumerable.Range(0, enemiesCount)
			.Select(i => CreateGenericEnemyByName("Goblin", shuffledTerrains[i].Position, _unitOfWork))
			.ToList();
		}

		private List<Agent> GeneratePlayers(List<string> characterNames, List<Position> startingPositions)
		{
			var randomizedPositions = Shuffle(startingPositions);
			var characters = Enumerable.Range(0, characterNames.Count())
			.Select(i => CreateAgentByName(characterNames[i], startingPositions[i], _unitOfWork))
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