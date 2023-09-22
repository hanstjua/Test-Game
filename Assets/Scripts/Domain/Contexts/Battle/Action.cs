using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    [Serializable]
    public abstract class Action : ValueObject<string>
    {
        public string Name { get; private set; }


        public Action(string name)
        {
            Name = name;
        }

        public override string Value()
        {
            return Name;
        }

        protected abstract ActionOutcome OnExecute(Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork);
        protected abstract bool ShouldExecute(Agent target, Agent actor);

        public abstract AreaOfEffect AreaOfEffect { get; }

        public abstract ActionType Type { get; }

        public bool IsTargetValid(Agent target, Agent actor)
        {
            return AreaOfEffect.RelativePositions.Contains(target.Position.RelativeTo(actor.Position))
            && ShouldExecute(target, actor);
        }

        public ActionOutcome[] Execute(Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            var potentialPreemptors = battle.PlayerIds
            .Concat(battle.EnemyIds)
            .Where(i => !i.Equals(actor.Id()))
            .OrderBy(i => unitOfWork.AgentRepository.Get(i).Stats.Agility)
            .Select(i => unitOfWork.AgentRepository.Get(i))
            .Append(actor)
            .Reverse()
            .ToArray();

            var outcomes = potentialPreemptors
            .SelectMany(p => new PreemptorService().Execute(p, potentialPreemptors.Where(a => !a.Equals(p)).ToArray(), Type, actor, targets, battle, unitOfWork))
            .ToArray();

            // refresh agents
            actor = unitOfWork.AgentRepository.Get(actor.Id() as AgentId);
            targets = targets.Select(a => unitOfWork.AgentRepository.Get(a.Id() as AgentId)).ToArray();

            if (IsTargetValid(targets[0], actor))
            {
                var executionOutcome = OnExecute(actor, targets, battle, unitOfWork);
                ApplyActionOutcomeService.Execute(executionOutcome, unitOfWork);
                
                outcomes = outcomes.Append(executionOutcome).ToArray();

                // refresh agents
                actor = unitOfWork.AgentRepository.Get(actor.Id() as AgentId);
                targets = targets.Select(a => unitOfWork.AgentRepository.Get(a.Id() as AgentId)).ToArray();

                var respondOutcomes = potentialPreemptors
                .SelectMany(p => new RespondService().Execute(p, potentialPreemptors.Where(a => !a.Equals(p)).ToArray(), executionOutcome, actor, targets, battle, unitOfWork))
                .ToArray();

                outcomes = outcomes.Concat(respondOutcomes).ToArray();
            }

            return outcomes;
        }
    }
}
