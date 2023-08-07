using System;
using System.Linq;
using Battle;
using UnityEngine;
using UnityEngine.Events;


public class SelectActionTarget : IUiState
{
    private Func<Agent, bool>  _targetValidator;
    private Func<Agent, IUiState> _actionHandler;
    private Func<IUiState> _cancelHandler;
    private bool _hasInit = false;
    private UnityAction<Position, Position> _characterPanelUpdater;

    public SelectActionTarget(Func<Agent, bool> targetValidator, Func<Agent, IUiState> actionHandler, Func<IUiState> cancelHandler)
    {
        _targetValidator = targetValidator;
        _actionHandler = actionHandler;
        _cancelHandler = cancelHandler;
    }

    private void Init(BattleProperties battleProperties)
    {
        _characterPanelUpdater = new((_, newPos) => UpdateCharacterPanel(battleProperties, newPos));
        battleProperties.cursor.SelectionChanged.AddListener(_characterPanelUpdater);

        var characterPanel = battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/CharacterPanel").gameObject;
        characterPanel.SetActive(true);

        _hasInit = true;
    }

    private void Uninit(BattleProperties battleProperties)
    {
        battleProperties.cursor.SelectionChanged.RemoveListener(_characterPanelUpdater);

        var characterPanel = battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/CharacterPanel").gameObject;
        characterPanel.SetActive(false);

        _hasInit = false;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit) Init(battleProperties);

        battleProperties.cursor.UpdateSelection();

        IUiState ret = this;
        if (Input.GetMouseButtonDown(0))
        {
            var position = battleProperties.cursor.Selection;
            var agent = battleProperties.unitOfWork.AgentRepository.GetAll().ToList().Find(a => a.Position.Equals(position) && _targetValidator(a));
            if (agent != null)
            {
                Uninit(battleProperties);
                ret = _actionHandler(agent);
            }
        }
        else if (Input.GetMouseButton(1))
        {
            Uninit(battleProperties);
            ret = _cancelHandler();
        }

        return ret;
    }

    private void UpdateCharacterPanel(BattleProperties battleProperties, Position position)
    {
        var agentRepository = battleProperties.unitOfWork.AgentRepository;
        var agent = agentRepository.GetFirstBy(a => a.Position.Equals(position));

        if (agent != null)
        {
            battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/CharacterPanel").GetComponent<CharacterPanel>().Character = agent;
        }
    }
}