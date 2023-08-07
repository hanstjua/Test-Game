using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Battle;


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
        Name = "nope";
        _instance = this;

        unitOfWorkObject.obj = new UnitOfWork(new AgentRepository(), new BattleRepository(), new BattleFieldRepository());
    }

    public static CompositionRoot Instance
    {
        get { return _instance; }
    }

    public void Yeet()
    {
        Name = "yeet";
    }

    public string Name { get; private set; }
}
