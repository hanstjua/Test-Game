using Battle.Statuses;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#nullable enable

namespace Battle
{
    public abstract class Equipment : ValueObject<string>
    {
        public enum HolderRole
        {
            Initiator,
            Reactor,
            Observer
        }

        public abstract EquipmentType Type { get; }
        public abstract string Description { get; }

        public virtual Stats StatsBoost => new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        public virtual ElementType[]? Elements => null;
        public virtual StatusType[]? Statuses => null;

        public Equipment() {}

        public static HolderRole GetHolderRole(AgentId holderId, AgentId actorId, AgentId[] targetIds)
        {
            if (holderId.Equals(actorId))
            {
                return HolderRole.Initiator;
            }
            else if (targetIds.Contains(holderId))
            {
                return HolderRole.Reactor;
            }
            else
            {
                return HolderRole.Observer;
            }
        }

        public ActionOutcome[] ApplyPreExecutionEffects(HolderRole role, Agent[] potentialPreemptors, ActionType action, Agent holder, Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            if (IsPreExecutionInitiatorEffectsTriggered(action, holder, targets, battle, unitOfWork))
            {
                // handle preempting observers

                // handle preexecution effects

                return null;
            }
            else return new ActionOutcome[] {};
        }

        public ActionOutcome GetPreExecutionEffects(HolderRole role, ActionType action, Agent holder, Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return role switch
            {
                HolderRole.Initiator => OnApplyPreExecutionInitiatorEffects(action, holder, targets, battle, unitOfWork),
                HolderRole.Reactor => OnApplyPreExecutionReactorEffects(action, actor, holder, battle, unitOfWork),
                HolderRole.Observer => OnApplyPreExecutionObserverEffects(action, holder, actor, targets, battle, unitOfWork),
                _ => throw new InvalidOperationException(string.Format("Unhandle role {0}", Enum.GetName(typeof(HolderRole), role))),
            };
        }

        public bool IsPreExecutionEffectsTriggered(HolderRole role, ActionType action, Agent holder, Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return role switch
            {
                HolderRole.Initiator => IsPreExecutionInitiatorEffectsTriggered(action, holder, targets, battle, unitOfWork),
                HolderRole.Reactor => IsPreExecutionReactorEffectsTriggered(action, actor, holder, battle, unitOfWork),
                HolderRole.Observer => IsPreExecutionObserverEffectsTriggered(action, holder, actor, targets, battle, unitOfWork),
                _ => throw new InvalidOperationException(string.Format("Unhandle role {0}", Enum.GetName(typeof(HolderRole), role))),
            };
        }

        public ActionOutcome GetPostExecutionEffects(HolderRole role, ActionOutcome outcome, Agent holder, Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return role switch
            {
                HolderRole.Initiator => OnApplyPostExecutionInitiatorEffects(outcome, holder, targets, battle, unitOfWork),
                HolderRole.Reactor => OnApplyPostExecutionReactorEffects(outcome, actor, holder, battle, unitOfWork),
                HolderRole.Observer => OnApplyPostExecutionObserverEffects(outcome, holder, actor, targets, battle, unitOfWork),
                _ => throw new InvalidOperationException(string.Format("Unhandle role {0}", Enum.GetName(typeof(HolderRole), role))),
            };
        }

        public bool IsPostExecutionEffectsTriggered(HolderRole role, ActionOutcome outcome, Agent holder, Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return role switch
            {
                HolderRole.Initiator => IsPostExecutionInitiatorEffectsTriggered(outcome, holder, targets, battle, unitOfWork),
                HolderRole.Reactor => IsPostExecutionReactorEffectsTriggered(outcome, actor, holder, battle, unitOfWork),
                HolderRole.Observer => IsPostExecutionObserverEffectsTriggered(outcome, holder, actor, targets, battle, unitOfWork),
                _ => throw new InvalidOperationException(string.Format("Unhandle role {0}", Enum.GetName(typeof(HolderRole), role))),
            };
        }

        protected virtual bool IsPreExecutionInitiatorEffectsTriggered(ActionType action, Agent holder, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return false;
        }

        protected virtual ActionOutcome OnApplyPreExecutionInitiatorEffects(ActionType action, Agent holder, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return null;
        }

        protected virtual bool IsPostExecutionInitiatorEffectsTriggered(ActionOutcome outcome, Agent holder, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return false;
        }

        protected virtual ActionOutcome OnApplyPostExecutionInitiatorEffects(ActionOutcome outcome, Agent holder, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return null;
        }

        protected virtual bool IsPreExecutionReactorEffectsTriggered(ActionType action, Agent actor, Agent holder, Battle battle, UnitOfWork unitOfWork)
        {
            return false;
        }

        protected virtual ActionOutcome OnApplyPreExecutionReactorEffects(ActionType action, Agent actor, Agent holder, Battle battle, UnitOfWork unitOfWork)
        {
            return null;
        }

        protected virtual bool IsPostExecutionReactorEffectsTriggered(ActionOutcome outcome, Agent actor, Agent holder, Battle battle, UnitOfWork unitOfWork)
        {
            return false;
        }

        protected virtual ActionOutcome OnApplyPostExecutionReactorEffects(ActionOutcome outcome, Agent actor, Agent holder, Battle battle, UnitOfWork unitOfWork)
        {
            return null;
        }

        protected virtual bool IsPreExecutionObserverEffectsTriggered(ActionType action, Agent holder, Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return false;
        }

        protected virtual ActionOutcome OnApplyPreExecutionObserverEffects(ActionType action, Agent holder, Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return null;
        }

        protected virtual bool IsPostExecutionObserverEffectsTriggered(ActionOutcome outcome, Agent holder, Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return false;
        }

        protected virtual ActionOutcome OnApplyPostExecutionObserverEffects(ActionOutcome outcome, Agent holder, Agent actor, Agent[] targets, Battle battle, UnitOfWork unitOfWork)
        {
            return null;
        }
    }

    public class EquipmentFactory
    {
        public static readonly Dictionary<string, Equipment> Instances = 
        AppDomain.CurrentDomain
        .GetAssemblies()
        .SelectMany(a => a.GetTypes())
        .Where(t => t.IsSubclassOf(typeof(Equipment)) && !t.GetTypeInfo().IsAbstract)
        .ToDictionary(t => t.Name, t => (Equipment) Activator.CreateInstance(t));
    }

    public class HandheldType : EquipmentType
    {
        public HandheldType(string name) : base(name)
        {}
    }

    public abstract class Handheld : Equipment
    {}
}