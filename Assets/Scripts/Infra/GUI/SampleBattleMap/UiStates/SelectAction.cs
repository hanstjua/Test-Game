using Battle;
using System;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class SelectAction : IUiState
{
    private static ActionDispatcher _actionDispatcher = new ActionDispatcher();

    private AgentId _agentId;
    private IUiState _nextState;
    private Func<BattleProperties, IUiState> _onCancel;
    private string _chosenAction;
    private bool _hasInit = false;
    private Transform _actionPanel;
    private CharacterPanel _characterPanel;

    public SelectAction(AgentId agentId, IUiState nextState, Func<BattleProperties, IUiState> onCancel)
    {
        _agentId = agentId;
        _nextState = nextState;
        _onCancel = onCancel;
    }

    private void Init(BattleProperties battleProperties)
    {
        Camera.main.GetComponent<CameraControl>().FocusAt(battleProperties.map.ToUIPosition(battleProperties.unitOfWork.AgentRepository.Get(_agentId).Position));

        _actionPanel = battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/ActionPanel");
        _actionPanel.GetComponent<ActionPanel>().ActionSelected.AddListener(a => _chosenAction = a.Name);
        _actionPanel.GetComponent<CanvasGroup>().alpha = 1;

        _characterPanel = battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/CharacterPanel").GetComponent<CharacterPanel>();
        _characterPanel.UpdateChracterPanelByAgent(battleProperties.unitOfWork.AgentRepository.Get(_agentId));

        var camera = Camera.main;

        LeanTween.value(camera.gameObject, x => camera.orthographicSize = x, 6f, 4f, 0.2f).setEase(LeanTweenType.easeOutExpo);

        _hasInit = true;
    }

    private void Uninit(BattleProperties battleProperties)
    {
        _actionPanel.GetComponent<CanvasGroup>().alpha = 0f;

        var camera = Camera.main;

        LeanTween.value(camera.gameObject, x => camera.orthographicSize = x, 4f, 6f, 0.2f).setEase(LeanTweenType.easeInExpo);

        _characterPanel.Hide();

        _chosenAction = null;

        _hasInit = false;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit) Init(battleProperties);

        if (_chosenAction != null)
        {            
            battleProperties.map.ClearHighlights();

            var action = _actionDispatcher.Dispatch(_chosenAction);

            var ret = new SelectActionTarget(action, new SelectAction(_agentId, _nextState, _onCancel), _nextState);

            Uninit(battleProperties);

            return ret;
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            Uninit(battleProperties);

            return _onCancel(battleProperties);
        }
        else
        {
            return this;
        }
    }
}