using System;
using System.Collections.Generic;

namespace Battle
{
	[Serializable]
	public class Battle : Entity
	{
		private BattleId _id;
		private int _turnCount;

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
			_turnCount = turnCount;
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

		public Battle NextTurn(AgentId nextAgent)
		{
			ActiveAgent = nextAgent;
			_turnCount += 1;

			return this;
		}

		public Battle NextPhase()
		{
			Phase = Phase.Transition(this);

			return this;
		}
	}
}
