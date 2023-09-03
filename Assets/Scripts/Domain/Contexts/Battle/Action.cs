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

        public abstract AreaOfEffect AreaOfEffect { get; }

        public abstract ActionType Type { get; }

        public bool IsTargetValid(Position targetPosition, Position actorPosition)
        {
            return AreaOfEffect.RelativePositions.Contains(targetPosition.RelativeTo(actorPosition));
        }
    }
}
