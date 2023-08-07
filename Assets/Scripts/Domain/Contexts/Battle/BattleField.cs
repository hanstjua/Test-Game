using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
	[Serializable]
	public class BattleField : Entity
	{
		private BattleFieldId _id;

		public BattleField(
			BattleFieldId id, 
			Terrain[][] terrains
		)
		{
			_id = id;
			Terrains = terrains;
		}

		public override object Id()
		{
			return _id;
		}

		public Terrain[][] Terrains { get; private set; }

		public int Width => Terrains[0].Count();
		public int Height => Terrains.Count();

		public List<Position> PlayerStartingPositions { 
			get {
				return Terrains
				.Aggregate(new Terrain[] {}, (acc, arr) => acc.Concat(arr).ToArray())
				.Where(t => t.IsStartingPosition)
				.Select(t => t.Position)
				.ToList();
			}
		}
	}
}
