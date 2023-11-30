using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle;
using Codice.CM.SEIDInfo;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    [SerializeField] public BattleEvents battleEvents;
    [SerializeField] public UnitOfWorkObject unitOfWorkObject;

    public AgentId agentId;
    public Map map;
    
    // Start is called before the first frame update
    void Start()
    {
        battleEvents.characterMoved.AddListener((a, b) => SetPosition(a, b));

        if (agentId == null) throw new Exception("agentId has not been set.");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameObject Create(AgentId id, Map map, string resourcePath, Vector3 position)
    {
        var ret = Instantiate((GameObject) Resources.Load(resourcePath), position + new Vector3(0, 0.1f, 0), Quaternion.identity);
        var character = ret.GetComponent<Character>();
        character.agentId = id;
        character.map = map;

        return ret;
    }

    public Character SetPosition(AgentId id, Position to)
    {
        if (id == agentId)
        {
            using (var unitOfWork = unitOfWorkObject.obj)
            {
                var agent = unitOfWork.AgentRepository.Get(agentId).Move(to);

                unitOfWork.AgentRepository.Update(agentId, agent);

                unitOfWork.Save();
            }

            transform.position = map.ToUIPosition(to);
        }

        return this;
    }

    public Character SetDirection(AgentId id, Direction towards)
    {
        if (id == agentId)
        {
            using (var unitOfWork = unitOfWorkObject.obj)
            {
                var agent = unitOfWork.AgentRepository.Get(agentId).Face(towards);

                unitOfWork.AgentRepository.Update(agentId, agent);

                unitOfWork.Save();
            }

            Vector2 direction = towards switch
            {
                Direction.North => new Vector2(0, 1),
                Direction.East => new Vector2(1,0),
                Direction.South => new Vector2(0, -1),
                Direction.West => new Vector2(-1,0)
            };

            transform.eulerAngles = new Vector3(0, (direction.x * 90) + (direction.y * (1 - direction.y) * 90), 0);
        }

        return this;
    }
}
