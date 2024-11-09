using System.Collections;
using System.Collections.Generic;
using Battle;
using UnityEngine;

public class AttackExecutionUi : MonoBehaviour
{
    public float damageScaler;
    public float positionScaler;
    public Character target;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Animate(BattleProperties battleProperties, AgentId actordId, AgentId targetId, ActionEffect[] outcomes)
    {

    }

    public bool IsAnimationPlaying()
    {
        return _animator.GetBool("play");
    }
}
