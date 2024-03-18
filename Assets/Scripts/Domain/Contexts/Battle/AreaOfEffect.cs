using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public class AreaOfEffect : ValueObject<(int, int)>
    {
        public AreaOfEffect(Position[] relativePositions, int height)
        {
            RelativePositions = relativePositions;
            Height = height;
        }

        public Position[] RelativePositions { get; private set; }  // XY positions relative to actor
        public int Height { get; private set; }  // height relative to actor

        public override (int, int) Value()
        {
            var serializedPositions = RelativePositions.Select(p => p.Value());
            var hashedPositions = ((IStructuralEquatable)serializedPositions).GetHashCode(EqualityComparer<(int, int, int)>.Default);

            return (hashedPositions, Height);
        }

        public bool IsWithin(Position p0, Position p1)
        {
            return RelativePositions.Contains(p1.RelativeTo(p0));
        }
    }
}