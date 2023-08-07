using System;

namespace Battle
{
    [Serializable]
    public class Terrain : ValueObject<ValueTuple<TerrainType, (int, int, int)>>
    {
        public Terrain(
			TerrainType type, 
			Position position, 
			bool isStartingPosition = false,
			bool traversable = true
		)
        {
            Type = type;
            Position = position;
            IsStartingPosition = isStartingPosition;
            Traversable = traversable;
        }

        public override ValueTuple<TerrainType, (int, int, int)> Value()
        {
            return (
                Type,
                Position.Value()
            );
        }

        public TerrainType Type { get; private set; }

        public Position Position { get; private set; }

        public bool IsStartingPosition { get; private set; }

		public bool Traversable { get; private set; }
    }
}
