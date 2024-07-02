using System;
using System.Collections.Generic;
using UnityEngine;
using Battle;
using Battle.SampleBattle;
using System.Linq;
using Battle.Weapons;
using Battle.Shieds;
using Battle.Armours;
using Battle.Footwears;
using Battle.Accessories;
using Battle.Services.Arbella;
using Common;

public class Main : MonoBehaviour
{
    [SerializeField] public UnitOfWorkObject unitOfWork;
    [SerializeField] public GameObject uiObjects;
    [SerializeField] public BattleEvents battleEvents;

    private const float INPUT_INTERVAL = 0.1f;

    private SampleBattleMap _sampleBattleMap;
    private Cursor _cursor;
    private BattleId _battleId;
    private BattleFieldId _battleFieldId;
    private Dictionary<AgentId, GameObject> _characters = new();
    private Map _map;
    private IUiState _uiState;
    private float _counter = 0;

    public GameObject CursorPrefab { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        var uow = unitOfWork.obj;
        using (uow)
        {
            _battleFieldId = uow.BattleFieldRepository.CreateBattleField(_sampleBattleMap.GetTerrains(_map.ToDomainPosition));
            uow.Save();
        }

        _cursor.BattleFieldId = _battleFieldId;

        _battleId = new Setup(uow, _battleFieldId).Execute();

        var battle = unitOfWork.obj.BattleRepository.Get(_battleId);

        foreach (var id in battle.EnemyIds.Concat(battle.PlayerIds))
        {
            var agent = uow.AgentRepository.Get(id);
            var character = Character.Create(id, _map, "Prefabs/Characters/Character", _map.ToUIPosition(agent.Position), battleEvents, unitOfWork.obj);
            character.LoadSprites(agent.Name);

            _characters.Add(id, character.gameObject);
        }

        foreach (var p in uow.BattleFieldRepository.Get(_battleFieldId).PlayerStartingPositions)
        {
            _map.Highlight(_map.ToUIPosition(p));
        }

        // set cursor to first character
        _cursor.Selection = uow.AgentRepository.Get(battle.PlayerIds[0]).Position;

        _uiState = new CharacterSelection();
    }

    // Update is called once per frame
    void Update()
    {
        if (_counter >= INPUT_INTERVAL)
        {
            var oldState = _uiState;
            _uiState = _uiState.Update(
                new BattleProperties(
                    unitOfWork.obj,
                    _characters,
                    _map,
                    _battleId,
                    _cursor,
                    uiObjects,
                    battleEvents
                )
            );

            if (oldState != _uiState) 
            {
                Debug.Log(_uiState.GetType());
                _counter = 0;
            }

            if (_uiState == new VictoryScreen())
            {
                Application.Quit();
            }
        }
        else
        {
            _counter += Time.deltaTime;
        }
    }

    void Awake()
    {
        _sampleBattleMap = uiObjects.GetComponentInChildren<SampleBattleMap>();

        _characters = new();

        _map = _sampleBattleMap.GetComponent<Map>();

        var prefab = Resources.Load("Prefabs/Maps/Cursor") as GameObject;

        _cursor = Cursor.Create(prefab, _map);
    }
}
