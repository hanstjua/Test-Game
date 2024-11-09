using Battle;
using Battle.Services.Arbella;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSelection : IUiState
{
    private readonly AgentId _selectedAgentId;
    private AgentId _testingAgentId;
    private bool _hasInit = false;
    private UnityAction<Position, Position> _characterPanelUpdater;

    public CharacterSelection(AgentId selectedAgentId = null)
    {
        _selectedAgentId = selectedAgentId;
    }

    private void Init(BattleProperties battleProperties)
    {
        var characterPanel = battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/CharacterPanel/Panel").GetComponent<CharacterPanel>();
        _characterPanelUpdater = new((_, newPos) => characterPanel.UpdateCharacterPanelByPosition(battleProperties, newPos));
        battleProperties.battleEvents.cursorSelectionChanged.AddListener(_characterPanelUpdater);

        _hasInit = true;
    }

    private void Uninit(BattleProperties battleProperties)
    {
        battleProperties.battleEvents.cursorSelectionChanged.RemoveListener(_characterPanelUpdater);

        battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/CharacterPanel/Panel").GetComponent<CanvasGroup>().alpha = 0;
        battleProperties.cursor.Selection = Cursor.NullSelection;

        _hasInit = false;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit)
        {
            Init(battleProperties);
        }

        Camera.main.GetComponent<CameraControl>().HandleCameraInput();

        battleProperties.cursor.UpdateSelection();

        IUiState ret = this;
        
        if (Input.GetKeyDown(KeyCode.Return)) ret = OnKeyPress(KeyCode.Return, battleProperties);
        else if (Input.GetKeyDown(KeyCode.Space)) ret = OnKeyPress(KeyCode.Space, battleProperties);
        else if (Input.GetKeyDown(KeyCode.UpArrow)) ret = OnKeyPress(KeyCode.UpArrow, battleProperties);
        else if (Input.GetKeyDown(KeyCode.C)) ret = OnKeyPress(KeyCode.C, battleProperties);
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
                    agent.Teleport(position);
                    
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
            return new CharacterSelectionConfirmation(battleProperties.uiObjects, this);

            case KeyCode.C:
            // var speechBubbleBottom = battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/SpeechBubbleBottom").GetComponent<SpeechBubble>();
            // speechBubbleBottom.AttachToCharacter(battleProperties.characters[_selectedAgentId].GetComponent<Character>());

            // var speechBubbleTop = battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/SpeechBubbleTop").GetComponent<SpeechBubble>();
            // speechBubbleTop.AttachToCharacter(battleProperties.characters[_selectedAgentId].GetComponent<Character>());
            
            var newBubble = SpeechBubble.Create(SpeechBubble.Variant.Bottom, battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage"));
            newBubble.AttachToCharacter(battleProperties.characters[_selectedAgentId].GetComponent<Character>());
            newBubble.PlayText("This is a test of long speech text that plays after a button press from the player, while the character may or may not be moving on the map itself.",
            0.02,
            SpeechBubble.Affiliation.Enemy,
            Resources.Load<Sprite>("UI/Battle/a_portrait_of_young_male_knight__-_deformed_iris__deformed_pupils__semi_realistic__cgi__3d__render__sketch__cartoon__drawing__anime___text__cropped__out_of_frame__worst_quality__low_quali_74522291"));


            Debug.Log("Character attached");
            return this;

            case KeyCode.Space:
            var characterScreen = battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/CharacterScreen").GetComponent<CharacterScreen>();
            
            if (characterScreen.gameObject.activeSelf) characterScreen.Hide();
            else {
                if (_testingAgentId == null)
                {
                    var id = new AgentId(Guid.NewGuid().ToString());
                    var agent = new Agent(
                        id,
                        "Anton",
                        new Arbellum[] {new Physical(0, true), new Malediction(0, false)},
                        new(100, 100, 100, 100, 100, 100, 100, 100, 3000, 1000),
                        new(1, 2, 3),
                        new(),
                        2,
                        null, null, null, null, null, null
                    );
                    var uow = battleProperties.unitOfWork;
                    using (uow)
                    {
                        uow.AgentRepository.Update(id, agent);
                        uow.Save();
                    }
                    _testingAgentId = id;
                }
                
                characterScreen.SetCharacter(
                    battleProperties.unitOfWork.AgentRepository.Get(_testingAgentId),
                    battleProperties.unitOfWork
                );
                characterScreen.Show();
            }

            return this;
            
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