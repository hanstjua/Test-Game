using System;
using System.Collections.Generic;
using Battle;

namespace Battle.Services.Actions
{
    public class Defend : Action
    {
        public override Position[] AreaOfEffect => throw new NotImplementedException();
        public override string Name => "Defend";
        public static ActionOutcome[] Execute(Agent actor)
        {
            actor.AddStatus(Status.Defend);

            return new ActionOutcome[] { new ActionOutcome((AgentId)actor.Id(), addStatuses: new Status[] {Status.Defend})};
        }
    }
}