using Battle.Statuses;
using Common;
using System;
using System.Linq;

#nullable enable

namespace Battle
{
    [Serializable]
    public abstract class Action : ValueObject<string>
    {
        public ActionType Type { get; private set; }
        public string Description { get; private set; }

        public Action(ActionType type, string description)
        {
            Type = type;
            Description = description;
        }

        public override string Value()
        {
            return Type.Name;
        }

        public override string ToString()
        {
            return Type.Name;
        }

        // for damage calc: maps (actor relevant stats - target relevant stats) to damage
        public static double DamageActivation(double statsDelta)
        {
            return 10000 / (1 + Math.Pow(Math.E, -(statsDelta - 200.0) / 50.0));
        }

        public abstract AreaOfEffect TargetArea { get; }
        public abstract AreaOfEffect AreaOfEffect { get; }
        public abstract ArbellumType Arbellum { get; }
        public abstract ActionPrerequisite[] Criteria { get; }
        public virtual StatType[] ActorRelevantStats => new StatType[] {};
        public virtual StatType[] TargetRelevantStats => new StatType[] {};
        public virtual int Cost => 0;
        public virtual ElementType[]? Elements => null;
        public virtual StatusType[]? Statuses => null;

        public abstract bool IsActorAbleToExecute(Agent actor, Battle battle, UnitOfWork unitOfWork);
        protected abstract ActionOutcome OnExecute(Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork);
        protected abstract bool ShouldExecute(Agent target, Agent actor);

        public bool CanExecute(Agent actor, Battle battle, UnitOfWork unitOfWork)
        {
            return actor.Mp >= Cost && IsActorAbleToExecute(actor, battle, unitOfWork);
        }

        private bool IsExecutionValid(Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return actor.Mp >= Cost;
        }

        public bool IsTargetValid(Agent target, Agent actor)
        {
            return TargetArea.IsWithin(actor.Position, target.Position)
            && ShouldExecute(target, actor);
        }

        public ActionOutcome[] Execute(Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            if (!IsExecutionValid(actor, targets, battle, unitOfWork)) throw new InvalidOperationException("Action execution is invalid");

            var potentialPreemptors = battle.PlayerIds
            .Concat(battle.EnemyIds)
            .Where(i => !i.Equals(actor.Id()))
            .OrderBy(i => unitOfWork.AgentRepository.Get(i).Stats.Agility)
            .Select(i => unitOfWork.AgentRepository.Get(i))
            .Append(actor)
            .Reverse()
            .ToArray();

            var outcomes = potentialPreemptors
            .SelectMany(
                p => new PreemptorService().Execute(
                    p,
                    potentialPreemptors.Where(a => !a.Equals(p)).ToArray(), 
                    Type, 
                    actor, 
                    targets, 
                    battle, 
                    unitOfWork
                )
            )
            .ToList();

            // refresh agents
            actor = unitOfWork.AgentRepository.Get(actor.Id() as AgentId);
            targets = targets.Select(a => unitOfWork.AgentRepository.Get(a.Id() as AgentId)).ToArray();

            if (IsTargetValid(targets[0], actor))
            {
                var executionOutcome = OnExecute(actor, targets, battle, unitOfWork);
                ApplyActionOutcomeService.Execute(executionOutcome, unitOfWork);
                LevelUpService.Execute(executionOutcome, this, unitOfWork);

                // reduce MP
                using (unitOfWork)
                {
                    unitOfWork.AgentRepository.Update(actor.Id() as AgentId, actor.ReduceMp(Cost));
                    unitOfWork.Save();
                }

                outcomes.Add(executionOutcome);
                
                // refresh agents
                actor = unitOfWork.AgentRepository.Get(actor.Id() as AgentId);
                targets = targets.Select(a => unitOfWork.AgentRepository.Get(a.Id() as AgentId)).ToArray();

                var respondOutcomes = potentialPreemptors.SelectMany(
                    p => new RespondService().Execute(
                        p, 
                        potentialPreemptors.Where(a => !a.Equals(p)).ToArray(), 
                        executionOutcome, 
                        actor, 
                        targets, 
                        battle, 
                        unitOfWork
                    )
                )
                .ToList();
                
                outcomes.AddRange(respondOutcomes);
            }

            return outcomes.ToArray();
        }
    }
}
