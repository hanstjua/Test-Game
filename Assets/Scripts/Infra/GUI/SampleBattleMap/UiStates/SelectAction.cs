using Battle;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class SelectAction : IUiState
{
    private static ActionDispatcher _actionDispatcher = new ActionDispatcher();

    private AgentId _agentId;
    private IUiState _prevState;
    private IUiState _nextState;
    private string _chosenAction;
    private bool _hasInit = false;
    private Transform _actionPanel;
    private CharacterPanel _characterPanel;

    public SelectAction(AgentId agentId, IUiState prevState, IUiState nextState)
    {
        _agentId = agentId;
        _prevState = prevState;
        _nextState = nextState;
    }

    private void Init(BattleProperties battleProperties)
    {
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
        _actionPanel.GetComponent<CanvasGroup>().alpha = 0.3f;

        var camera = Camera.main;

        LeanTween.value(camera.gameObject, x => camera.orthographicSize = x, 4f, 6f, 0.2f).setEase(LeanTweenType.easeInExpo);

        _characterPanel.Hide();

        _hasInit = false;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        Camera.main.GetComponent<CameraControl>().FocusAt(battleProperties.map.ToUIPosition(battleProperties.unitOfWork.AgentRepository.Get(_agentId).Position));

        if (!_hasInit) Init(battleProperties);


        if (_chosenAction != null)
        {            
            _actionPanel.GetComponent<CanvasGroup>().alpha = 0;

            battleProperties.map.ClearHighlights();

            Uninit(battleProperties);

            var handler = _actionDispatcher.Dispatch(_chosenAction);

            return handler.Handle(battleProperties, this, _nextState);
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            Uninit(battleProperties);

            return _prevState;
        }
        else
        {
            return this;
        }
    }
}