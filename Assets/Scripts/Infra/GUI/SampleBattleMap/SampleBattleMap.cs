using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Battle;
 

public class SampleBattleMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Battle.Terrain[][] GetTerrains(Func<Vector3, Position> vector3ToPosition)
    {
        var terrains = new List<Battle.Terrain>();
        var maxX = 0;
        var maxY = 0;

        for (int i = 0; i < transform.childCount; i++)
        {
            var layer = transform.GetChild(i);
            for (int j = 0; j < layer.childCount; j++)
            {
                
                if (layer.GetChild(j).TryGetComponent<Block>(out var block))
                {
                    var position = vector3ToPosition(block.transform.position);

                    terrains.Add(
                        new Battle.Terrain(
                            block.Type,
                            position,
                            block.IsStartingPosition,
                            block.Traversable
                        )
                    );

                    maxX = position.X > maxX ? position.X : maxX;
                    maxY = position.Y > maxY ? position.Y : maxY;
                }
            }
        }

        var terrainMatrix = Enumerable.Range(0, maxX + 1).Select(i => Enumerable.Range(0, maxY + 1).Select(j => terrains.Where(t => t.Position.X == i && t.Position.Y == j).ToArray()).ToArray()).ToArray();

        Battle.Terrain GetHigherTerrain(Battle.Terrain t1, Battle.Terrain t2) => t1.Position.Z > t2.Position.Z ? t1 : t2;
        Battle.Terrain GetTopLevelTerrain(Battle.Terrain[] arr) => arr.Aggregate(GetHigherTerrain);

        return terrainMatrix
        .Select(row => row.Select(GetTopLevelTerrain).ToArray())
        .ToArray();
    }
}
