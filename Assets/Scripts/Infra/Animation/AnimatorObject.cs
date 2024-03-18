using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorObject : MonoBehaviour
{   
    private Animator _animator;
    private AnimationExecutor _executor;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Animate(AnimationExecutor executor)
    {
        _executor = executor;

        return executor.Execute();
    }

    public bool IsAnimating(AnimationExecutor executor)
    {
        return _executor == executor && _executor.IsAnimating;
    }
}
