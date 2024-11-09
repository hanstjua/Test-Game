using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cutscene.Sequence;
using UnityEngine;

public class PlayCutscene : IUiState
{
    private Sequence _sequence;
    private IUiState _nextState;
    private bool _isComplete = false;
    private bool _hasStarted = false;

    public PlayCutscene(Sequence sequence, IUiState nextState)
    {
        _sequence = sequence;
        _nextState = nextState;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasStarted)
        {
            battleProperties.map.StartCoroutine(PlaySequence(battleProperties));
            _hasStarted = true;
        }
        
        return _isComplete ? _nextState : this;
    }

    private IEnumerator PlaySequence(BattleProperties battleProperties)
    {
        var animationExecutors = _sequence.Actions
        .Select(action => AnimationExecutorFactory.Get(action, battleProperties))
        .ToArray();

        for (int i = 0; i < animationExecutors.Length; i++)
        {
            yield return new WaitForSeconds((float)_sequence.Actions[i].Interval);
            animationExecutors[i].Execute();
        }

        var lastExecutor = animationExecutors.Last();

        yield return new WaitUntil(() => !lastExecutor.IsAnimating);

        _isComplete = true;

        yield return null;
    }
}