using System;
using System.Collections;

namespace Battle
{
    [Serializable]
    public class Position : ValueObject<ValueTuple<int, int, int>>
    {
        public Position(
            int x,
            int y,
            int z
        )
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override ValueTuple<int, int, int> Value()
        {
            return (X, Y, Z);
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }

        public int MovementCost(Position to)
        {
            var deltaX = Math.Abs(X - to.X);
            var deltaY = Math.Abs(Y - to.Y);
            var deltaZ = Math.Abs(Z - to.Z);

            return deltaX + deltaY + deltaZ;
        }

        public double Distance(Position from)
        {
            var deltaX = X - from.X;
            var deltaY = Y - from.Y;
            var deltaZ = Z - from.Z;

            return Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2) + Math.Pow(deltaZ, 2));
        }

        public Position RelativeTo(Position target)
        {
            return new Position(X - target.X, Y - target.Y, Z - target.Z);
        }
    }
}