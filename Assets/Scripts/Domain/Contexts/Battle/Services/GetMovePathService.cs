using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public class GetMovePathService
    {
        private record Node
        {
            public Node(Position pos, double gCost, double hCost, Position previous)
            {
                Position = pos;
                GCost = gCost;
                HCost = hCost;
                Previous = previous;
            }

            public Position Position { get; private set; }
            public double GCost { get; private set; }
            public double HCost { get; private set; }
            public double FCost => GCost + HCost;
            public Position Previous { get; private set; }
        }
        public Position[] Execute(Position start, Position end, BattleField field, Position[] adversaryPositions)
        {
            var edges = new List<Node> {new Node(start, 0, start.Distance(end), null)};
            var inners = new List<Node>();

            while (edges.Count > 0 && !inners.Exists(i => i.Position == end))
            {
                var lowestCost = edges.Min(e => e.FCost);
                var current = edges.First(e => e.FCost == lowestCost);

                edges.Remove(current);
                inners.Add(current);

                // check neighbours
                foreach (var delta in new List<(int, int)> {(0, 1), (1, 0), (0, -1), (-1, 0)})
                {
                    var x = current.Position.X + delta.Item1;
                    var y = current.Position.Y + delta.Item2;
                    
                    // check that xy-coord is valid and the terrain is traversable
                    if (x >= 0 
                    && x < field.Width 
                    && y >= 0  
                    && y < field.Height 
                    && field.Terrains[x][y].Traversable
                    && !adversaryPositions.Contains(field.Terrains[x][y].Position))
                    {
                        var neighbourPosition = field.Terrains[x][y].Position;

                        if (neighbourPosition.Equals(end))
                        {
                            inners.Add(new Node(end, -1, -1, current.Position));
                            break;
                        }
                        else if (inners.Select(i => i.Position).Contains(neighbourPosition))
                        {
                            continue;
                        }

                        var gCost = neighbourPosition.Distance(start);
                        var hCost = neighbourPosition.Distance(end);
                        var newNode = new Node(neighbourPosition, gCost, hCost, current.Position);

                        if (edges.Exists(e => e.Position.Equals(neighbourPosition)))
                        {
                            var existingNode = edges.Find(e => e.Position == neighbourPosition);
                            if (existingNode.FCost > newNode.FCost)
                            {
                                edges[edges.IndexOf(existingNode)] = newNode;
                            }
                        }
                        else
                        {
                            edges.Add(newNode);
                        }
                    }
                }
            }

            return TracePath(inners, start, end).ToArray();
        }

        private List<Position> TracePath(List<Node> nodes, Position start, Position end)
        {
            var ret = new List<Position>();
            var current = nodes.Find(n => n.Position.Equals(end));

            while (true)
            {
                ret.Insert(0, current.Position);
                current = nodes.Find(n => n.Position == current.Previous);
                if (current.Position == start)
                {
                    break;
                }
            }

            return ret;
        }

        private List<Node> GetNeighbourNodes(Node current, BattleField field, Position start, Position end)
        {
            return new List<(int, int)> {(0, 1), (1, 0), (0, -1), (-1, 0)}
                .Select(xy => (xy.Item1 + current.Position.X, xy.Item2 + current.Position.Y))
                .Where(xy => (
                    (xy.Item1 >= 0 && xy.Item1 < field.Width) 
                    && (xy.Item2 >= 0  && xy.Item2 < field.Height) 
                    && field.Terrains[xy.Item1][xy.Item2].Traversable
                    ))
                .Select(xy => field.Terrains[xy.Item1][xy.Item2].Position)
                .Select(p => new Node(p, p.Distance(start), p.Distance(end), current.Position))
                .ToList();
        }
    }
}