using System;
using System.Collections.Generic;

namespace Battle
{
	[Serializable]
	public class Battle : Entity
	{
		private readonly BattleId _id;

		public Battle(
			BattleId id,
			List<AgentId> players,
			List<AgentId> enemies,
			BattleFieldId battleFieldId,
			Phase phase,
			int turnCount = 0)
		{
			_id = id;
			PlayerIds = players;
			EnemyIds = enemies;
			BattleFieldId = battleFieldId;
			Phase = phase;
			TurnCount = turnCount;
		}

		public override object Id()
		{
			return _id;
		}

		public List<AgentId> PlayerIds { get; private set; }

		public List<AgentId> EnemyIds { get; private set; }

		public BattleFieldId BattleFieldId { get; private set; }

		public Phase Phase { get; private set; }

		public AgentId ActiveAgent { get; private set; }

		public int TurnCount { get; private set; }

		public Battle NextTurn(AgentId nextAgent)
		{
			ActiveAgent = nextAgent;
			TurnCount += 1;

			return this;
		}

		public Battle NextPhase()
		{
			Phase = Phase.Transition(this);

			return this;
		}
	}
}
