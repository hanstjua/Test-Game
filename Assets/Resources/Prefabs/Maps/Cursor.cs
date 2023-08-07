using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Battle;

public class Cursor : MonoBehaviour
{
    [field: SerializeField] public UnitOfWorkObject UnitOfWork { get; private set; }
    [field: SerializeField] public GameObject CursorPrefab { get; private set; }

    public BattleFieldId BattleFieldId { get; set; }

    public UnityEvent<Position, Position> SelectionChanged { get; private set; }

    private Camera _mainCamera;
    private Map _map;
    private Position _selection;

    // Start is called before the first frame update
    void Start()
    {
        CursorPrefab = Instantiate(CursorPrefab, new Vector3(-1, -1, -1), CursorPrefab.transform.rotation);
        SelectionChanged = new();
        SelectionChanged.AddListener(ShowCursor);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        _mainCamera = Camera.main;
        _map = GetComponent<Map>();
    }

    public Position Selection
    {
        get => _selection == null? new Position(-1, -1, -1) : _selection;

        set
        {
            if (!Selection.Equals(value))
            {
                SelectionChanged.Invoke(_selection, value);
                _selection = value;
            }
        }
    }

    public void ActivateSelection()
    {
        CursorPrefab.transform.Rotate(new Vector3(0, 0, 45));
    }

    public void DeactivateSelection()
    {
        CursorPrefab.transform.Rotate(new Vector3(0, 0, -45));
    }

    public void Reset()
    {
        DeactivateSelection();
        _selection = null;
    }

    public void UpdateSelection()
    {
        // handle canvas and screen resolution difference
        var renderTexture = _mainCamera.targetTexture;
        var mouseOnRenderTexture = new Vector3(Input.mousePosition.x * (float)renderTexture.width / (float)Screen.width, Input.mousePosition.y * (float) renderTexture.height / (float) Screen.height);

        var ray = _mainCamera.ScreenPointToRay(mouseOnRenderTexture);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                var position = _map.ToDomainPosition(hit.collider.gameObject.transform.position);
                var battleField = UnitOfWork.obj.BattleFieldRepository.Get(BattleFieldId);

                var terrain = battleField.Terrains[position.X][position.Y];
                Selection = terrain.Traversable ? terrain.Position : Selection;
            }
        }
    }

    private void ShowCursor(Position oldPosition, Position newPosition)
    {
        CursorPrefab.transform.position = _map.ToUIPosition(newPosition) + new Vector3(0, 0.2f, 0);
    }
}
