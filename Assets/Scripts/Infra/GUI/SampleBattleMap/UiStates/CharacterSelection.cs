using Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSelection : IUiState
{
    private AgentId _selectedAgentId;
    private bool _hasInit = false;
    private UnityAction<Position, Position> _characterPanelUpdater;

    public CharacterSelection(AgentId selectedAgentId = null)
    {
        _selectedAgentId = selectedAgentId;
    }

    private void Init(BattleProperties battleProperties)
    {
        var characterPanel = battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/CharacterPanel").GetComponent<CharacterPanel>();
        _characterPanelUpdater = new((_, newPos) => characterPanel.UpdateCharacterPanelByPosition(battleProperties, newPos));
        battleProperties.battleEvents.cursorSelectionChanged.AddListener(_characterPanelUpdater);

        _hasInit = true;
    }

    private void Uninit(BattleProperties battleProperties)
    {
        battleProperties.battleEvents.cursorSelectionChanged.RemoveListener(_characterPanelUpdater);

        battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/CharacterPanel").GetComponent<CanvasGroup>().alpha = 0;
        battleProperties.cursor.Selection = Cursor.NullSelection;

        _hasInit = false;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit)
        {
            Init(battleProperties);
        }

        battleProperties.cursor.UpdateSelection();

        IUiState ret = this;
        
        if (Input.GetKeyDown(KeyCode.Return)) ret = OnKeyPress(KeyCode.Return, battleProperties);
        else if (Input.GetKeyDown(KeyCode.UpArrow)) ret = OnKeyPress(KeyCode.UpArrow, battleProperties);
        else if (Input.GetMouseButtonDown(0)) ret = OnMouseClick(battleProperties.cursor.Selection, battleProperties);
        
        return ret;
    }

    public IUiState OnMouseClick(Position position, BattleProperties battleProperties)
    {
        IUiState ret = this;
        if (_selectedAgentId == null)
        {
            var battle = battleProperties.unitOfWork.BattleRepository.Get(battleProperties.battleId);
            var agent = battleProperties.unitOfWork.AgentRepository.GetFirstBy(a => a.Position.Equals(position) && battle.PlayerIds.Contains(a.Id()));
            if (agent != null)
            {
                battleProperties.cursor.ActivateSelection();

                Uninit(battleProperties);
                ret = new CharacterSelection(agent.Id() as AgentId);
            }
            
        }
        else
        {
            var battle = battleProperties.unitOfWork.BattleRepository.Get(battleProperties.battleId);
            var battleField = battleProperties.unitOfWork.BattleFieldRepository.Get(battle.BattleFieldId);

            if (battleField.PlayerStartingPositions.Contains(position))
            {
                using (var unitOfWork = battleProperties.unitOfWork)
                {
                    var agent = unitOfWork.AgentRepository.Get(_selectedAgentId);
                    agent.Position = position;
                    
                    unitOfWork.Save();
                }
                
                var character = battleProperties.characters[_selectedAgentId];
                character.transform.position = battleProperties.map.ToUIPosition(position) + Character.POSITION_OFFSET;

                battleProperties.cursor.DeactivateSelection();

                Uninit(battleProperties);

                ret = new CharacterSelection();
            }
        }

        return ret;
    }

    public IUiState OnKeyPress(KeyCode key, BattleProperties battleProperties)
    {
        switch (key)
        {
            case KeyCode.Return:
            Uninit(battleProperties);
            return new CharacterSelectionConfirmation(battleProperties.uiObjects,this);
            
            case KeyCode.UpArrow:
            return this;

            case KeyCode.DownArrow:
            return this;

            case KeyCode.LeftArrow:
            return this;

            case KeyCode.RightArrow:
            return this;

            default:
            return this;
        }
    }
}