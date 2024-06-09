using Common;
using UnityEngine;


public class CompositionRoot : MonoBehaviour
{
    private static CompositionRoot _instance;

    [field: SerializeField] public UnitOfWorkObject unitOfWorkObject { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        _instance = this;

        unitOfWorkObject.obj = new UnitOfWork(
            new AgentRepository(),
            new BattleRepository(),
            new BattleFieldRepository(),
            new InventoryRepository()
        );
    }

    public static CompositionRoot Instance
    {
        get => _instance;
    }
}
