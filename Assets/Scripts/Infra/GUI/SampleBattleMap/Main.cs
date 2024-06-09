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

            // populate inventory
            var inventory = uow.InventoryRepository.Get();
            inventory.AddItem(Item.Potion, 10);
            inventory.AddEquipment(new Longsword(), 3);
            inventory.AddEquipment(new Buckler(), 3);
            inventory.AddEquipment(new LeatherArmour(), 3);
            inventory.AddEquipment(new IronBoots(), 3);
            inventory.AddEquipment(new GoldNecklace(), 1);
            inventory.AddEquipment(new SilverRing(), 2);

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

    public Agent CreateAgentByName(string name, Position position, UnitOfWork unitOfWork)
    {
        var id = new AgentId(Guid.NewGuid().ToString());
        var agent = new Agent(
            id,
            name,
            new Arbellum[] {new Physical(0)},
            new StatLevels(100, 100, 100, 100, 100, 100, 100, 100, 3000, 1000),
            position,
            new Dictionary<Item, int>(),
            2,
            null, null, null, null, null, null
        );

        var service = new EquipService();
        service.Execute(id, new Longsword(), false, unitOfWork);
        service.Execute(id, new LeatherArmour(), unitOfWork);
        service.Execute(id, new IronBoots(), unitOfWork);
        service.Execute(id, new GoldNecklace(), true, unitOfWork);

        using (unitOfWork)
        {
            unitOfWork.AgentRepository.Update(id, agent);
            unitOfWork.Save();
        }

        return agent;
    }

    public Agent CreateGenericEnemyByName(string name, Position position, UnitOfWork unitOfWork)
    {
        var character = CharacterFactory.GetGeneric(name);

        var id = new AgentId(Guid.NewGuid().ToString());
        var agent = new Agent(
            id, 
            name, 
            character.Arbella, 
            character.Levels, 
            position, 
            character.Items, 
            character.Movements,
            null, null, null, null, null, null
        );
        
        var service = new EquipService();
        service.Execute(id, character.LeftHand, false, unitOfWork);
        service.Execute(id, character.RightHand, true, unitOfWork);
        service.Execute(id, character.Armour, unitOfWork);
        service.Execute(id, character.Footwear, unitOfWork);
        service.Execute(id, character.Accessory1, true, unitOfWork);
        service.Execute(id, character.LeftHand, false, unitOfWork);

        using (unitOfWork)
        {
            unitOfWork.AgentRepository.Update(id, agent);
            unitOfWork.Save();
        }

        return agent;
    }
}
