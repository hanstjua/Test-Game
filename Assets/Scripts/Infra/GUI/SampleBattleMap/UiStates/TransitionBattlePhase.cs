using System.Linq;
using Battle;
using UnityEngine;
using TMPro;


public class TransitionBattlePhase : IUiState
{
    private GameObject _anim;
    private ActionOutcome[] _toAnimate;
    private float _elapsed = 0;
    private float _period = 2;
    private bool _hasInit = false;
    private Agent _activeAgent;

    private void Init(BattleProperties battleProperties)
    {
        using (var unitOfWork = battleProperties.unitOfWork)
        {
            var battle = unitOfWork.BattleRepository.Get(battleProperties.battleId);
            Debug.Log(battle.TurnCount);
            unitOfWork.BattleRepository.Update(battleProperties.battleId, battle.NextPhase());
            _activeAgent = unitOfWork.AgentRepository.Get(battle.ActiveAgent);

            unitOfWork.Save();
        }

        _anim = GameObject.Find("CameraCanvas/RawImage/GenericStatusAnim");
        var statuses = _activeAgent.Statuses;

        if (statuses.Count > 0)
        {
            var outcomes = statuses
            .Select(s => s.Apply(_activeAgent, battleProperties.unitOfWork.BattleRepository.Get(battleProperties.battleId), battleProperties.unitOfWork))
            .Aggregate((acc, next) => acc.Concat(next).ToArray());

            battleProperties.battleEvents.statusApplied.Invoke(outcomes);

            _toAnimate = outcomes;
        }

        _hasInit = true;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit) Init(battleProperties);

        if (_toAnimate != null)
        {
            _anim.GetComponent<CanvasGroup>().alpha = 1;
            if (Animate(battleProperties))
            {
                _toAnimate = _toAnimate.Count() > 1 ? _toAnimate.Skip(1).ToArray() : null;
                _elapsed = 0;
                _anim.GetComponent<CanvasGroup>().alpha = 0;
            }

            return this;
        }

        var battle = battleProperties.unitOfWork.BattleRepository.Get(battleProperties.battleId);
        if (battle.Phase.Equals(new Victory(battleProperties.unitOfWork)))
        {
            return new VictoryScreen();
        }
        // handle when phase is victory/game over 
        else return new CharacterTurn(_activeAgent.Id() as AgentId, false, false, _activeAgent.Position);
    }

    private bool Animate(BattleProperties battleProperties)
    {
        var outcome = _toAnimate[0];

        var text = _anim.GetComponent<TMP_Text>();
        text.text = "Effects -> ";

        var agent = battleProperties.unitOfWork.AgentRepository.Get(outcome.On);

        text.text += agent.Name + ":\n";

        if (outcome.HpDamage != 0)
        {
            if (outcome.HpDamage > 0)
            {
                text.text += "HP -" + outcome.HpDamage.ToString() + "\n";
            }
            else
            {
                text.text += "HP +" + (-outcome.HpDamage).ToString() + "\n";
            }
        }

        if (outcome.MpDamage != 0)
        {
            if (outcome.MpDamage > 0)
            {
                text.text += "MP -" + outcome.MpDamage.ToString() + "\n";
            }
            else
            {
                text.text += "Mp +" + (-outcome.MpDamage).ToString() + "\n";
            }
        }

        if (outcome.AddStatuses != null)
        {
            foreach(var status in outcome.AddStatuses)
            {
                text.text += status.Name + "added\n";
            }
        }

        if (outcome.RemoveStatuses != null)
        {
            foreach(var status in outcome.RemoveStatuses)
            {
                text.text += status.Name + " removed\n";
            }
        }

        _anim.GetComponent<CanvasGroup>().alpha = 1;

        _elapsed += Time.deltaTime;
        var angle = 2 * Mathf.PI / _period * _elapsed;
        _anim.GetComponent<TMP_Text>().alpha = Mathf.Sin(angle);

        return angle >= Mathf.PI;
    }
}