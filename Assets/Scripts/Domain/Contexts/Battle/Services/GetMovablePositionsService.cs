using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public class GetMovablePositionsService
    {
        private record Direction
        {
            public Direction(int x, int y, int dx, int dy)
            {
                X = x;
                Y = y;
                Dx = dx;
                Dy = dy;
            }

            public int X { get; private set; }
            public int Y { get; private set; }
            public int Dx { get; private set; }
            public int Dy { get; private set; }
        }

        private record Node
        {
            public Node(Position pos, double cost)
            {
                Position = pos;
                Cost = cost;
            }

            public double Cost { get; private set; }
            public Position Position { get; private set; }
        }

        public Position[] Execute(Agent agent, BattleField field)
        {
            var agentPos = agent.Position;
            var edges = new List<Node> {new Node(agentPos, 0)};
            var inners = new List<Node>();

            while (edges.Count > 0)
            {
                var lowestCost = edges.Min(e => e.Cost);
                var current = edges.First(e => e.Cost == lowestCost);

                edges.Remove(current);
                inners.Add(current);

                // check neighbours
                foreach (var delta in new List<(int, int)> {(0, 1), (1, 0), (0, -1), (-1, 0)})
                {
                    var x = current.Position.X + delta.Item1;
                    var y = current.Position.Y + delta.Item2;
                    
                    // check that xy-coord is valid and the terrain is traversable
                    if ((x >= 0 && x < field.Width) && (y >= 0  && y < field.Height) && field.Terrains[x][y].Traversable)
                    {
                        var curPos = field.Terrains[x][y].Position;
                        var cost = curPos.Distance(agentPos);

                        // position is movable
                        if (cost <= agent.Movements)
                        {
                            var adjacent = new Node(curPos, cost);
                            edges.Add(adjacent);
                        }
                    }
                }
            }

            return inners.Select(e => e.Position).ToArray();
        }
    }
}