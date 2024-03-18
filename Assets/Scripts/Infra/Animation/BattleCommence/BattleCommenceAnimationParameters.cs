using System.Collections;
using System.Collections.Generic;
using Battle;
using UnityEngine;

public class BattleCommenceAnimationParameters : MonoBehaviour
{
    public float textAlphaScaler;
    public float canvasAlphaScaler;
    public BattleCommenceAnimationExecutor executor;

    public BattleProperties battleProperties;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Trigger()
    {
        
    }

    public void UpdateAnimationStatus(bool isAnimating)
    {
        executor.UpdateAnimationStatus(isAnimating);
    }
}
