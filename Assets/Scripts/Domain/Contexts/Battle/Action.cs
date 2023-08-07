using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    [Serializable]
    public abstract class Action : ValueObject<string>
    {
        public abstract string Name { get; }

        public override string Value()
        {
            return Name;
        }

        public abstract Position[] AreaOfEffect { get; }

        public bool IsTargetValid(Position targetPosition, Position actorPosition)
        {
            return AreaOfEffect.Contains(targetPosition.RelativeTo(actorPosition));
        }
    }
}
