using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;
using Battle.SampleBattle;
using UnityEngine.UI;
using TMPro;
using System.Text.Json;
using Unity.Plastic.Newtonsoft.Json;

public class Main : MonoBehaviour
{
    [SerializeField] public UnitOfWorkObject unitOfWork;
    [SerializeField] public GameObject uiObjects;
    [SerializeField] public BattleEvents battleEvents;

    private SampleBattleMap _sampleBattleMap;
    private Cursor _cursor;
    private BattleId _battleId;
    private BattleFieldId _battleFieldId;
    private Camera _mainCamera;
    private Dictionary<AgentId, GameObject> _characters = new();
    private Map _map;
    private IUiState _uiState;

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

        foreach (var id in battle.EnemyIds)
        {
            var enemy = uow.AgentRepository.Get(id);
            var ser = JsonUtility.ToJson(enemy);
            Debug.Log($"main {ser}");
            Debug.Log($"{JsonUtility.FromJson<Agent>(ser).Name}");
            var character = Character.Create(id, _map, "Prefabs/Characters/Character", _map.ToUIPosition(enemy.Position), battleEvents, unitOfWork.obj);
            character.LoadSprites(enemy.Name);

            _characters.Add(id, character.gameObject);
        }

        foreach (var id in battle.PlayerIds)
        {
            var player = uow.AgentRepository.Get(id);
            var character = Character.Create(id, _map, "Prefabs/Characters/Character", _map.ToUIPosition(player.Position), battleEvents, unitOfWork.obj);
            character.LoadSprites(player.Name);

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

        if (oldState != _uiState) Debug.Log(_uiState.GetType());

        if (_uiState == new VictoryScreen())
        {
            Application.Quit();
        }
    }

    void Awake()
    {
        _sampleBattleMap = uiObjects.GetComponentInChildren<SampleBattleMap>();

        _characters = new();

        _mainCamera = Camera.main;

        _map = _sampleBattleMap.GetComponent<Map>();

        var prefab = Resources.Load("Prefabs/Maps/Cursor") as GameObject;

        _cursor = Cursor.Create(prefab, _map);
    }
}
