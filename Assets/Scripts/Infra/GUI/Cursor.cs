using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Battle;

public class Cursor : MonoBehaviour
{
    [SerializeField] public BattleEvents battleEvents;
    
    [field: SerializeField] public UnitOfWorkObject UnitOfWork { get; private set; }
    [field: SerializeField] private float _heightOffset = 0.3f;

    public static readonly Position NullSelection = new Position(-10, -10, -10);
    public BattleFieldId BattleFieldId { get; set; }

    private Camera _mainCamera;
    private Map _map;
    private Position _selection;
    private ColorPalette _palette;

    public static Cursor Create(GameObject prefab, Map map)
    {
        var gameObject = Instantiate(prefab, new Vector3(-10, -10, -10), prefab.transform.rotation);
        var cursor = gameObject.GetComponent<Cursor>();
        cursor.Init(map);

        return cursor;
    }

    // Start is called before the first frame update
    void Start()
    {
        _palette = ColorPalette.Get();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void Init(Map map)
    {
        _map = map;

        battleEvents.cursorSelectionChanged.AddListener(ShowCursor);
    }

    public Position Selection
    {
        get => _selection ?? NullSelection;

        set
        {
            var newSelection = value ?? NullSelection;
            if (!Selection.Equals(newSelection))
            {
                var oldSelection = _selection;
                _selection = newSelection;
                battleEvents.cursorSelectionChanged.Invoke(oldSelection, _selection);
            }
        }
    }

    public void ActivateSelection()
    {
        GetComponent<Renderer>().sharedMaterial.SetColor("_Color", _palette.GetColor("Green1"));        
    }

    public void DeactivateSelection()
    {
        GetComponent<Renderer>().sharedMaterial.SetColor("_Color", _palette.GetColor("Green3"));
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
                Selection = terrain.Traversable ? terrain.Position : NullSelection;
            }
        }
        else
        {
            Selection = NullSelection;
        }
    }

    private void ShowCursor(Position oldPosition, Position newPosition)
    {
        transform.position = _map.ToUIPosition(newPosition) + new Vector3(0, _heightOffset, 0);
    }
}
