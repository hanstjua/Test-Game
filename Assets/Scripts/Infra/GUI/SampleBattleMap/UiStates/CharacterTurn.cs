using Battle;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.UI;
using Codice.CM.Triggers;

public class CharacterTurn : IUiState
{
    private static ActionDispatcher _actionDispatcher = new ActionDispatcher();
    private static BattleField _field;

    private AgentId _agentId;
    private string _chosenAction;
    private bool _hasInit = false;
    private bool _hasMoved = false;
    private bool _hasActed = false;
    private Position _initialPosition;
    private Direction _initialDirection;
    private Transform _actionPanel;
    private Position[] _movablePositions;

    public CharacterTurn(AgentId agentId, bool hasMoved, bool hasActed, Position initialPosition, Direction initialDirection)
    {
        _agentId = agentId;
        _hasMoved = hasMoved;
        _hasActed = hasActed;
        _initialPosition = initialPosition;
        _initialDirection = initialDirection;
    }

    private void Init(BattleProperties battleProperties)
    {
        var agent = battleProperties.unitOfWork.AgentRepository.Get(_agentId);

        _actionPanel = battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/ActionsPanel");

        if (_actionPanel.childCount == 0 && !_hasActed)
        {
            _actionPanel.GetComponent<CanvasGroup>().alpha = 1;

            var buttonObj = GameObject.Find("Button");
            foreach(var action in agent.Actions)
            {
                var obj = GameObject.Instantiate(buttonObj, _actionPanel);
                var text = obj.transform.GetComponentInChildren<Text>();

                text.text = action.Name;

                // make UI Element call ActionDispatcher.Dispatch() on click.
                var button = obj.GetComponent<Button>();
                button.onClick.AddListener(() => _chosenAction = action.Name);
            }

            // add end turn button
            var endTurnObj = GameObject.Instantiate(buttonObj, _actionPanel);
            var endTurnText = endTurnObj.transform.GetComponentInChildren<Text>();

            endTurnText.text = "End Turn";

            // make UI Element call ActionDispatcher.Dispatch() on click.
            var endTurnButton = endTurnObj.GetComponent<Button>();
            endTurnButton.onClick.AddListener(() => _chosenAction = "End Turn");
        }

        var battle = battleProperties.unitOfWork.BattleRepository.Get(battleProperties.battleId);

        _field ??= battleProperties.unitOfWork.BattleFieldRepository.Get(battle.BattleFieldId);

        var playerIds = battle.PlayerIds;
        var enemyIds = battle.EnemyIds;
        var isPlayer = playerIds.Contains(agent.Id() as AgentId);
        var allyIds = isPlayer ? playerIds.Where(i => !i.Equals(agent.Id())) : enemyIds.Where(i => !i.Equals(agent.Id()));
        var adversaryIds = isPlayer? enemyIds : playerIds;
        var allyPositions = allyIds.Select(i => battleProperties.unitOfWork.AgentRepository.Get(i).Position);
        var adversaryPositions = adversaryIds.Select(i => battleProperties.unitOfWork.AgentRepository.Get(i).Position);

        _movablePositions = new GetMovablePositionsService().Execute(agent, _field, allyPositions.ToArray(), adversaryPositions.ToArray());

        _hasInit = true;
    }

    private void Uninit(BattleProperties battleProperties)
    {
        battleProperties.map.ClearHighlights();

        _actionPanel.GetComponent<CanvasGroup>().alpha = 0;
        _actionPanel.DetachChildren();

        _hasInit = false;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit) Init(battleProperties);

        if (_chosenAction != null)
        {
            for (int i = 0; i < _actionPanel.childCount; i++)
            {
                GameObject.Destroy(_actionPanel.GetChild(i).gameObject);
            }
            
            _actionPanel.GetComponent<CanvasGroup>().alpha = 0;

            battleProperties.map.ClearHighlights();

            Uninit(battleProperties);

            var handler = _actionDispatcher.Dispatch(_chosenAction);
            IUiState onProceedState = _hasMoved ? new SelectDirection(_agentId) : new CharacterTurn(_agentId, false, true, _initialPosition, _initialDirection);

            return handler.Handle(battleProperties, new CharacterTurn(_agentId, _hasMoved, false, _initialPosition, _initialDirection), onProceedState);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            return OnMouseClick(battleProperties);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            return OnCancelMove(battleProperties);
        }
        else
        {
            if (!_hasMoved)
            {
                battleProperties.cursor.UpdateSelection();

                foreach(var position in _movablePositions)
                {
                    var map = battleProperties.map;
                    map.Highlight(map.ToUIPosition(position));
                }
            }
            else
            {
                battleProperties.cursor.Selection = null;
            }

            return this;
        }
    }

    private IUiState OnCancelMove(BattleProperties battleProperties)
    {
        if (_hasMoved)
        {
            battleProperties.characters[_agentId]
            .GetComponent<Character>()
            .SetPosition(_agentId, _initialPosition)
            .SetDirection(_agentId, _initialDirection);
        }

        Uninit(battleProperties);

        return new CharacterTurn(_agentId, false, _hasActed, _initialPosition, _initialDirection);
    }

    private IUiState OnMouseClick(BattleProperties battleProperties)
    {
        IUiState ret = this;

        if (_hasActed && _movablePositions.Contains(battleProperties.cursor.Selection))
        {
            Uninit(battleProperties);
                
            ret = new CharacterMovement(
                _agentId, 
                battleProperties.cursor.Selection, 
                new SelectDirection(_agentId, _initialPosition, OnCancelMove)
            );
        }
        else if (_movablePositions.Contains(battleProperties.cursor.Selection))
        {
            Uninit(battleProperties);

            ret = new CharacterMovement(
                _agentId,
                battleProperties.cursor.Selection,
                new CharacterTurn(_agentId, true, _hasActed, _initialPosition, _initialDirection)
            );
        }

        return ret;
    }
}