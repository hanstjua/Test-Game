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
        battleEvents.actionExecuted.AddListener(ApplyActionOutcomes);
        battleEvents.statusApplied.AddListener(ApplyStatusOutcomes);
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

    private void ApplyActionOutcomes(ActionEffect[] outcomes)
    {
        var unitOfWork = unitOfWorkObject.obj;
        using (unitOfWork)
        {
            var agent = unitOfWork.AgentRepository.Get(agentId);

            foreach(var outcome in outcomes.Where(outcome => outcome.On == agentId))
            {
                if (outcome.HpDamage > 0) agent = agent.ReduceHp((int) outcome.HpDamage);
                else if (outcome.HpDamage < 0) agent = agent.RestoreHp((int) -outcome.HpDamage);

                if (outcome.MpDamage > 0) agent = agent.ReduceMp((int) outcome.MpDamage);
                else if (outcome.MpDamage < 0) agent = agent.RestoreMp((int) -outcome.MpDamage);

                if (outcome.AddStatuses != null)
                {
                    foreach(var status in outcome.AddStatuses)
                    {
                        agent = agent.AddStatus(status);
                    }
                }

                if (outcome.RemoveStatuses != null)
                {
                    foreach(var status in outcome.AddStatuses)
                    {
                        agent = agent.RemoveStatus(status);
                    }
                }
            }

            unitOfWork.AgentRepository.Update(agentId, agent);

            unitOfWork.Save();
        }
    }

    private void ApplyStatusOutcomes(ActionEffect[] outcomes)
    {
        foreach(var outcome in outcomes)
        {
            if (outcome.On == agentId)
            {
                var unitOfWork = unitOfWorkObject.obj;
                using (unitOfWork)
                {
                    var agent = unitOfWork.AgentRepository.Get(outcome.On);

                    if (outcome.RemoveStatuses != null)
                    {
                        foreach(var status in outcome.RemoveStatuses)
                        {
                            agent.RemoveStatus(status);
                        }
                    }

                    if (outcome.AddStatuses != null)
                    {
                        foreach(var status in outcome.AddStatuses)
                        {
                            agent.AddStatus(status);
                        }
                    }

                    unitOfWork.Save();
                }
            }
        }
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
