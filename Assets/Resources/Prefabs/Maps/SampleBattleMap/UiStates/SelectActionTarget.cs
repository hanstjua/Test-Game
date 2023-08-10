using System;
using System.Linq;
using Battle;
using UnityEngine;
using UnityEngine.Events;


public class SelectActionTarget : IUiState
{
    private IActionHandler _actionHandler;
    private bool _hasInit = false;
    private UnityAction<Position, Position> _characterPanelUpdater;

    public SelectActionTarget(IActionHandler actionHandler)
    {
        _actionHandler = actionHandler;
    }

    private void Init(BattleProperties battleProperties)
    {
        var characterPanel = battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/CharacterPanel").GetComponent<CharacterPanel>();
        _characterPanelUpdater = new((_, newPos) => characterPanel.UpdateCharacterPanel(battleProperties, newPos));
        battleProperties.battleEvents.cursorSelectionChanged.AddListener(_characterPanelUpdater);

        

        _hasInit = true;
    }

    private void Uninit(BattleProperties battleProperties)
    {
        battleProperties.battleEvents.cursorSelectionChanged.RemoveListener(_characterPanelUpdater);

        battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/CharacterPanel").gameObject.SetActive(false);
        battleProperties.cursor.Selection = new Position(-1, -1, -1);

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
            var agent = battleProperties.unitOfWork.AgentRepository.GetAll().ToList().Find(a => a.Position.Equals(position) && _actionHandler.ValidateTarget(a));
            if (agent != null)
            {
                Uninit(battleProperties);
                ret = _actionHandler.ExecuteAction(agent);
            }
        }
        else if (Input.GetMouseButton(1))
        {
            Uninit(battleProperties);
            ret = _actionHandler.CancelAction();
        }

        return ret;
    }
}