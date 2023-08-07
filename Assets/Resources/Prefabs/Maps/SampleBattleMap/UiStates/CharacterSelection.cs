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
                var agent = battleProperties.unitOfWork.AgentRepository.Get(_selectedAgentId);
                using (battleProperties.unitOfWork)
                {
                    agent.Position = position;
                    battleProperties.unitOfWork.Save();
                }
                
                var character = battleProperties.characters[_selectedAgentId];
                character.transform.position = battleProperties.map.ToUIPosition(position) + new Vector3(0, battleProperties.map.Offset.y + character.transform.localScale.y / 2, 0);

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
            Debug.Log("enter pressed");
            Uninit(battleProperties);
            return new CharacterSelectionConfirmation(battleProperties.uiObjects,this);
            
            case KeyCode.UpArrow:
            Debug.Log("Up pressed");
            return this;

            case KeyCode.DownArrow:
            Debug.Log("Down pressed");
            return this;

            case KeyCode.LeftArrow:
            Debug.Log("Left pressed");
            return this;

            case KeyCode.RightArrow:
            Debug.Log("Arrow pressed");
            return this;

            default:
            return this;
        }
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