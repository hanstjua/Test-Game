using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;
using Battle.SampleBattle;
using UnityEngine.UI;
using TMPro;

public class Main : MonoBehaviour
{
    [SerializeField] public UnitOfWorkObject unitOfWork;
    [SerializeField] public GameObject uiObjects;

    private SampleBattleMap _sampleBattleMap;
    private Cursor _cursor;
    private BattleId _battleId;
    private BattleFieldId _battleFieldId;
    private Camera _mainCamera;
    private Dictionary<AgentId, GameObject> _characters = new Dictionary<AgentId, GameObject>();
    private Map _map;
    private IUiState _uiState;

    // Start is called before the first frame update
    void Start()
    {
        using (unitOfWork.obj)
        {

            _battleFieldId = unitOfWork.obj.BattleFieldRepository.CreateBattleField(_sampleBattleMap.GetTerrains(_map.ToDomainPosition));
            unitOfWork.obj.Save();
        }

        _cursor.BattleFieldId = _battleFieldId;

        _battleId = new Setup(unitOfWork.obj, _battleFieldId).Execute();

        var battle = unitOfWork.obj.BattleRepository.Get(_battleId);

        foreach (var id in battle.EnemyIds)
        {
            var enemy = unitOfWork.obj.AgentRepository.Get(id);
            var obj = Instantiate((GameObject) Resources.Load("Prefabs/Characters/Stair"), _map.ToUIPosition(enemy.Position) + new Vector3(0, 0.1f, 0), Quaternion.identity);

            _characters.Add(id, obj);
        }

        foreach (var id in battle.PlayerIds)
        {
            var player = unitOfWork.obj.AgentRepository.Get(id);
            var obj = Instantiate((GameObject) Resources.Load("Prefabs/Characters/Stair"), _map.ToUIPosition(player.Position) + new Vector3(0, 0.1f, 0), Quaternion.identity);

            _characters.Add(id, obj);
        }

        foreach (var p in unitOfWork.obj.BattleFieldRepository.Get(_battleFieldId).PlayerStartingPositions)
        {
            _map.Highlight(_map.ToUIPosition(p));
        }

        // set cursor to first character
        _cursor.Selection = unitOfWork.obj.AgentRepository.Get(battle.PlayerIds[0]).Position;

        _uiState = new CharacterSelection();
    }

    // Update is called once per frame
    void Update()
    {
        _uiState = _uiState.Update(
            new BattleProperties(
                unitOfWork.obj,
                _characters,
                _map,
                _battleId,
                _cursor,
                uiObjects
            )
        );
    }

    void Awake()
    {
        uiObjects.transform.Find("CameraCanvas").Find("RawImage").Find("ConfirmSelectionPanel").gameObject.SetActive(false);

        _cursor = GetComponent<Cursor>();

        _sampleBattleMap = uiObjects.GetComponentInChildren<SampleBattleMap>();

        _characters = new();

        _mainCamera = Camera.main;

        _map = GetComponent<Map>();
        _map.CalculateOffset(_sampleBattleMap.gameObject);
        _map.CalculateScale(_sampleBattleMap.gameObject);
    }
}
