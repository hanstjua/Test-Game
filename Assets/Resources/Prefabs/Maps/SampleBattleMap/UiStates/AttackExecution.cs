using Battle;
using Battle.Services.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AttackExecution : IUiState
{
    private static ActionDispatcher _actionDispatcher = new ActionDispatcher();
    private ActionOutcome[] _outcomes;
    private GameObject _anim;
    private float _elapsed = 0;
    private float _period = 1;

    public AttackExecution(ActionOutcome[] outcomes)
    {
        _outcomes = outcomes;
        _anim = GameObject.Find("CameraCanvas/RawImage/AttackAnim");
        _anim.GetComponent<TMP_Text>().text = outcomes[0].HpDamage.ToString();
        _anim.SetActive(true);
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        _elapsed += Time.deltaTime;
        var angle = 2 * Mathf.PI / _period * _elapsed;
        if (angle >= Mathf.PI)
        {
            _anim.SetActive(false);

            // transition battle into next phase
            Agent activeAgent;
            using (battleProperties.unitOfWork)
            {
                var battle = battleProperties.unitOfWork.BattleRepository.Get(battleProperties.battleId);
                battleProperties.unitOfWork.BattleRepository.Update(battleProperties.battleId, battle.NextPhase());
                activeAgent = battleProperties.unitOfWork.AgentRepository.Get(battle.ActiveAgent);
            }

            return new CharacterTurn(activeAgent);
        }
        else
        {
            _anim.GetComponent<TMP_Text>().alpha = Mathf.Sin(angle);
            return this;
        }
    }
}