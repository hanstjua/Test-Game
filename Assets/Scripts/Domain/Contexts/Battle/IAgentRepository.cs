﻿using System;

namespace Battle
{
	public interface IAgentRepository
	{
		public Agent Get(AgentId id);
		public Agent[] GetAll();
		public Agent GetFirstBy(Predicate<Agent> predicate);
		public bool Remove(AgentId id);
		public Agent Update(AgentId id, Agent newAgent);
		public IAgentRepository Reload();
		public IAgentRepository Save();
	}
}
