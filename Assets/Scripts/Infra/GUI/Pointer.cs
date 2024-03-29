using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Battle;
using System.Linq;

public class Pointer : MonoBehaviour
{
    [field: SerializeField] private float _heightOffset = 2f;

    public static readonly Position NullSelection = new(-100, -100, -100);
    public BattleFieldId BattleFieldId { get; set; }

    private Camera _mainCamera;
    private Map _map;

    public Position Position
    {
        get => _map.ToDomainPosition(transform.position - new Vector3(0, _heightOffset, 0));
        set => transform.position = _map.ToUIPosition(value) + new Vector3(0, _heightOffset, 0);
    }

    public static Pointer Create(GameObject prefab, Map map)
    {
        var gameObject = Instantiate(prefab, new Vector3(-100, -100, -100), prefab.transform.rotation);
        var pointer = gameObject.GetComponent<Pointer>();
        pointer.Init(map);

        return pointer;
    }

    // Start is called before the first frame update
    void Start()
    {
        Reset();
        LeanTween.rotateAround(gameObject, Vector3.up, 360, 4f).setRepeat(-1);
        LeanTween.value(gameObject, 0, -1, 0.5f)
        .setOnUpdate((float val) => transform.position = _map.ToUIPosition(Position) + new Vector3(0, (0.3f * val) + _heightOffset, 0))
        .setLoopPingPong()
        .setEase(LeanTweenType.easeInOutSine);
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
    }

    public void Reset()
    {
        Position = NullSelection;
    }

    public void Destroy()
    {
        LeanTween.cancel(gameObject);
        GameObject.Destroy(gameObject);
    }
}
