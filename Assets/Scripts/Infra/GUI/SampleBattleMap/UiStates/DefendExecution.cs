using Battle;
using Battle.Services.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DefendExecution : IUiState
{
    private ActionEffect _outcome;
    private GameObject _anim;
    private float _elapsed = 0;
    private float _period = 1;
    private bool _hasInit = false;
    private IUiState _nextState;

    public DefendExecution(ActionEffect outcome, IUiState nextState)
    {
        _outcome = outcome;
        _nextState = nextState;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit)
        {
            _anim = GameObject.Find("CameraCanvas/RawImage/DefendAnim");

            var text = _anim.GetComponent<TMP_Text>();
            text.text = "";

            text.text += battleProperties.unitOfWork.AgentRepository.Get(_outcome.On).Name + "\n";
            text.text += "Defend\n";

            _anim.GetComponent<CanvasGroup>().alpha = 1;

            _hasInit = true;
        }  

        if (Animate())  // animation complete
        {
            return _nextState;
        }
        else
        {
            return this;
        }
    }

    private bool Animate()
    {
        _elapsed += Time.deltaTime;
        var angle = 2 * Mathf.PI / _period * _elapsed;
        _anim.GetComponent<TMP_Text>().alpha = Mathf.Sin(angle);

        return angle >= Mathf.PI;
    }
}