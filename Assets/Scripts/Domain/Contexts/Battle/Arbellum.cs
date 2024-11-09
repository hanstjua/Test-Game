using System;
using System.Linq;

namespace Battle
{
    public abstract class Arbellum : ValueObject<string>
    {
        public const int MAX_EXP = 999;

        public override string Value()
        {
            return Type.Name;
        }

        public readonly struct Learnable
        {
            public Action Action { get; }
            public int RequiredExperience { get; }
            public bool IsPassive { get; }

            public Learnable(Action action, int requiredExperience, bool isPassive)
            {
                if (requiredExperience < 0) throw new ArgumentException($"Required Experience must be >= 0. (Got {requiredExperience} instead)");

                Action = action;
                RequiredExperience = requiredExperience;
                IsPassive = isPassive;
            }
        }

        public ArbellumType Type { get; private set; }
        public string Description { get; private set; }
        public int Experience { get; private set; }
        public Learnable[] Learnables { get;  private set; }
        public Action[] Actives { get; private set; }
        public Action[] Passives { get; private set; }
        public bool IsActive { get; private set; }
        public virtual int MaxMpCost => Experience / 3;  // TODO: figure out proper cost formula
        
        public Arbellum(ArbellumType type, string description, int experience, Learnable[] learnables, bool isActive)
        {
            Type = type;
            Description = description;
            Experience = experience;
            Learnables = learnables;
            IsActive = isActive;

            Actives = new Action[] {};
            Passives = new Action[] {};

            Learn();
        }

        private void Learn()
        {
            // learn actives
            Actives = Actives
            .Concat(
                Learnables
                .Where(l => !l.IsPassive && l.RequiredExperience <= Experience)
                .Select(l => l.Action)
            ).ToArray();

            // learn passives
            Passives = Passives
            .Concat(
                Learnables
                .Where(l => l.IsPassive && l.RequiredExperience <= Experience)
                .Select(l => l.Action)
            ).ToArray();

            // remove learned learnables
            Learnables = Learnables.Where(l => l.RequiredExperience > Experience).ToArray();
        }

        public Arbellum ExperienceUp(int points)
        {
            Experience = Math.Min(MAX_EXP, Experience + points);
            Learn();

            return this;
        }

        public Arbellum Activate()
        {
            IsActive = true;
            return this;
        }

        public Arbellum Deactivate()
        {
            IsActive = false;
            return this;
        }
    }
}