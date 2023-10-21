using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle;
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
        battleEvents.characterMoved.AddListener(MoveCharacter);

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

    public void MoveCharacter(AgentId id, Position to)
    {
        if (id == agentId)
        {
            using (var unitOfWork = unitOfWorkObject.obj)
            {
                var agent = unitOfWork.AgentRepository.Get(agentId);

                agent.Move(to);

                unitOfWork.AgentRepository.Update(agentId, agent);

                unitOfWork.Save();
            }

            transform.position = map.ToUIPosition(to);
        }
    }
}
