using System;
using System.Collections.Generic;
using Battle;

namespace Battle.Actions
{
    public class DefendService
    {
        public static ActionOutcome[] Execute(Agent actor)
        {
            actor.AddStatus(Status.Defend);

            return new ActionOutcome[] { new ActionOutcome((AgentId)actor.Id(), addStatuses: new Status[] {Status.Defend})};
        }
    }
}