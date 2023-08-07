using System;

namespace Battle
{
    [Serializable]
    public abstract class Phase : ValueObject<string>
    {
        public static string  Name { get; private set; }
        
        public Phase(string name)
        {
            Name = name;
        }

        public override string Value()
        {
            return Name;
        }

        public abstract Phase Transition(Battle battle);
    }
}