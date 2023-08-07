using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Battle;
using UnityEngine;

[Serializable]
public struct PositionData
{
    public int x;
    public int y;
    public int z;

    public PositionData(Position pos)
    {
        x = pos.X;
        y = pos.Y;
        z = pos.Z;
    }

    public Position ToPosition()
    {
        return new Position(x, y, z);
    }
}

[Serializable]
public struct TerrainData
{
    public TerrainType type;
    public PositionData positionData;
    public bool isStartingPosition;
    public bool traversable;

    public TerrainData(Battle.Terrain terrain)
    {
        type = terrain.Type;
        positionData = new PositionData(terrain.Position);
        isStartingPosition = terrain.IsStartingPosition;
        traversable = terrain.Traversable;
    }

    public Battle.Terrain ToTerrain()
    {
        return new Battle.Terrain(
            type,
            positionData.ToPosition(),
            isStartingPosition,
            traversable
        );
    }
}

[Serializable]
public struct BattleFieldIdData
{
    public string uuid;

    public BattleFieldIdData(BattleFieldId id)
    {
        uuid = id.Uuid;
    }

    public BattleFieldId ToBattleFieldId()
    {
        return new BattleFieldId(uuid);
    }
}

[Serializable]
public struct BattleFieldData
{
    public BattleFieldIdData battleFieldIdData;
    public TerrainData[][] terrainDataMatrix;

    public BattleFieldData(BattleField field)
    {
        battleFieldIdData = new BattleFieldIdData((BattleFieldId)field.Id());
        terrainDataMatrix = field.Terrains.Select(terrains => terrains.Select(t => new TerrainData(t)).ToArray()).ToArray();
    }

    public BattleField ToBattleField()
    {
        return new BattleField(
            battleFieldIdData.ToBattleFieldId(),
            terrainDataMatrix.Select(terrains => terrains.Select(t => t.ToTerrain()).ToArray()).ToArray()
        );
    }
}

public class BattleFieldRepository : IBattleFieldRepository
{
    private Dictionary<BattleFieldIdData, BattleFieldData> _tempBattleFieldSet = new Dictionary<BattleFieldIdData, BattleFieldData>();
    private MemoryStream _battleFieldSet;
    private BinaryFormatter _serializer = new BinaryFormatter();

    BattleField IBattleFieldRepository.GetBattleFieldByName(string name)
    {
        throw new NotImplementedException();
    }

    Battle.BattleField IBattleFieldRepository.Get(BattleFieldId id)
    {
        _battleFieldSet.Position = 0;
        var set = (Dictionary<BattleFieldIdData, BattleFieldData>) _serializer.Deserialize(_battleFieldSet);
        return set[new BattleFieldIdData(id)].ToBattleField();
    }

    IBattleFieldRepository IBattleFieldRepository.Reload()
    {
        if (_battleFieldSet != null)
        {
            _battleFieldSet.Position = 0;
            var ser = new BinaryFormatter();
            _tempBattleFieldSet = (Dictionary<BattleFieldIdData, BattleFieldData>) ser.Deserialize(_battleFieldSet);
        }

        return this;
    }

    IBattleFieldRepository IBattleFieldRepository.Save()
    {
        var mems = new MemoryStream();
        _serializer.Serialize(mems, _tempBattleFieldSet);
        _battleFieldSet = mems;

        return this;
    }

    BattleFieldId IBattleFieldRepository.CreateBattleField(Battle.Terrain[][] terrains)
    {
        var id = new BattleFieldId(Guid.NewGuid().ToString());
        _tempBattleFieldSet.Add(new BattleFieldIdData(id), new BattleFieldData(new BattleField(id, terrains)));
        return id;
    }
}