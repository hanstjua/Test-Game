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
    
    // Start is called before the first frame update
    void Start()
    {
        battleEvents.actionExecuted.AddListener(ApplyActionOutcomes);

        if (agentId == null) throw new Exception("agentId has not been set.");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static GameObject Create(AgentId id, string resourcePath, Vector3 position)
    {
        var ret = Instantiate((GameObject) Resources.Load(resourcePath), position + new Vector3(0, 0.1f, 0), Quaternion.identity);
        ret.GetComponent<Character>().agentId = id;

        return ret;
    }

    private void ApplyActionOutcomes(ActionOutcome[] outcomes)
    {
        var unitOfWork = unitOfWorkObject.obj;
        using (unitOfWork)
        {
            var agent = unitOfWork.AgentRepository.Get(agentId);

            foreach(var outcome in outcomes.Where(outcome => outcome.On == agentId))
            {
                if (outcome.HpDamage > 0) agent = agent.ReduceHp((int) outcome.HpDamage);
                else if (outcome.HpDamage < 0) agent = agent.RestoreHp((int) outcome.HpDamage);

                if (outcome.MpDamage > 0) agent = agent.ReduceMp((int) outcome.MpDamage);
                else if (outcome.MpDamage < 0) agent = agent.RestoreMp((int) outcome.MpDamage);

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
        }
    }
}
