using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    [Serializable]
    public abstract class Action : ValueObject<string>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Action(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public override string Value()
        {
            return Name;
        }

        public override string ToString()
        {
            return Name;
        }

        // for damage calc: maps (actor relevant stats - target relevant stats) to damage
        public static double DamageActivation(double statsDelta)
        {
            return (StatLevels.MAX_LEVEL + 1) / (1 + Math.Pow(Math.E, -(statsDelta - 200) / 50));
        }

        protected abstract ActionOutcome OnExecute(Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork);
        protected abstract bool ShouldExecute(Agent target, Agent actor);

        public abstract AreaOfEffect TargetArea { get; }
        public abstract AreaOfEffect AreaOfEffect { get; }

        public abstract ActionType Type { get; }
        public abstract SkillType Skill { get; }
        public abstract StatType[] ActorRelevantStats { get; }
        public abstract StatType[] TargetRelevantStats { get; }
        public abstract ActionPrerequisite[] Criteria { get; }

        public abstract bool CanExecute(Agent actor, Battle battle, UnitOfWork unitOfWork);

        public bool IsTargetValid(Agent target, Agent actor)
        {
            return TargetArea.IsWithin(actor.Position, target.Position)
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
