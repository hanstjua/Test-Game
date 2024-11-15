using Battle;
using Cutscene.Actions;
using Cutscene.Sequence;
using System;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class CharacterTurn : IUiState
{
    private static BattleField _field;

    private AgentId _agentId;
    private bool _hasInit = false;
    private bool _hasMoved = false;
    private bool _hasActed = false;
    private Position _initialPosition;
    private Direction _initialDirection;
    private Transform _actionPanel;
    private CameraControl _cameraControl;
    private Position[] _movablePositions;
    private Pointer _pointer;
    
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
        _pointer = Pointer.CreatePointer(battleProperties.map);

        var characterPanel = battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/CharacterPanel/Panel").GetComponent<CharacterPanel>();
        battleProperties.battleEvents.cursorSelectionChanged.AddListener((_, newPos) => characterPanel.UpdateCharacterPanelByPosition(battleProperties, newPos));

        _actionPanel = battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/ActionPanel");
        _actionPanel.GetComponent<CanvasGroup>().alpha = 0;

        var agent = battleProperties.unitOfWork.AgentRepository.Get(_agentId);

        var battle = battleProperties.unitOfWork.BattleRepository.Get(battleProperties.battleId);

        if (!_hasActed)
        {
            var actions = agent.Actions
            .ToDictionary(
                a => a, 
                a => a.CanExecute(agent, battle, battleProperties.unitOfWork)
            );

            _actionPanel.GetComponent<ActionPanel>().UpdateActions(actions);

            // TODO: add end turn button
        }

        _field ??= battleProperties.unitOfWork.BattleFieldRepository.Get(battle.BattleFieldId);

        var playerIds = battle.PlayerIds;
        var enemyIds = battle.EnemyIds;
        var isPlayer = playerIds.Contains(agent.Id() as AgentId);
        var allyIds = isPlayer ? playerIds.Where(i => !i.Equals(agent.Id())) : enemyIds.Where(i => !i.Equals(agent.Id()));
        var adversaryIds = isPlayer? enemyIds : playerIds;
        var allyPositions = allyIds.Select(i => battleProperties.unitOfWork.AgentRepository.Get(i).Position);
        var adversaryPositions = adversaryIds.Select(i => battleProperties.unitOfWork.AgentRepository.Get(i).Position);

        _movablePositions = new GetMovablePositionsService().Execute(agent, _field, allyPositions.ToArray(), adversaryPositions.ToArray());

        foreach(var position in _movablePositions)
        {
            var map = battleProperties.map;
            map.Highlight(map.ToUIPosition(position));
        }

        _cameraControl = Camera.main.transform.GetComponent<CameraControl>();
        _cameraControl.FocusAt(battleProperties.map.ToUIPosition(battleProperties.unitOfWork.AgentRepository.Get(_agentId).Position));

        _hasInit = true;
    }

    private void Uninit(BattleProperties battleProperties)
    {
        battleProperties.map.ClearHighlights();

        _actionPanel.GetComponent<CanvasGroup>().alpha = 0;

        _pointer.Destroy();

        _hasInit = false;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit) Init(battleProperties);

        var agent = battleProperties.unitOfWork.AgentRepository.Get(_agentId);

        _cameraControl.HandleCameraInput();

        if (Input.GetKey(KeyCode.F))
        {
            Camera.main.GetComponent<CameraControl>().FocusAtObject(battleProperties.characters[_agentId]);
        }

        // test play sequence
        if (Input.GetKey(KeyCode.C))
        {
            var seq = new Sequence
            {
                Actions = new Cutscene.Action[]
                {
                    new CameraMove
                    {
                        Interval = 0,
                        By = new Cutscene.Position
                        {
                            Coord = new int[] {1, 0}
                        },
                        Duration = 0.5
                    },
                    new CameraPan
                    {
                        Interval = 0.5,
                        Degrees = 45,
                        Duration = 0.6
                    },
                    new CameraPan
                    {
                        Interval = 0.7,
                        Degrees = 45,
                        Duration = 0.2
                    },
                    new Talk
                    {
                        Interval = 0.2,
                        Position = Talk.BubblePosition.bottom,
                        Target = "james",
                        Speech = "You scoundrel!",
                        Affiliation = Talk.SpeakerAffiliation.ally,
                        Duration = 10
                    },
                    new Talk
                    {
                        Interval = 2,
                        Position = Talk.BubblePosition.top,
                        Target = "jane",
                        Speech = "Hello, there. That's not a very nice way to greet a beautiful lady now, is it?",
                        Affiliation = Talk.SpeakerAffiliation.ally,
                        Duration = 8
                    }
                }
            };

            return new PlayCutscene(seq, this);
        }

        if (Input.GetMouseButtonDown(0))
        {
            return OnMouseClick(battleProperties);
        }

        var cursor = battleProperties.cursor;
        cursor.UpdateSelection();

        // show action panel preview if cursor is on agent
        if (cursor.Selection.Equals(agent.Position))
        {
            _pointer.Position = agent.Position;
        }
        else
        {
            _pointer.Position = Pointer.NullSelection;
        }

        return this;
    }

    private IUiState OnCancelMove(BattleProperties battleProperties)
    {
        // if (_hasMoved)
        // {
        //     battleProperties.characters[_agentId]
        //     .GetComponent<Character>()
        //     .SetPosition(_agentId, _initialPosition)
        //     .SetDirection(_agentId, _initialDirection);
        // }

        battleProperties.characters[_agentId]
        .GetComponent<Character>()
        .SetPosition(_agentId, _initialPosition)
        .SetDirection(_agentId, _initialDirection);

        // Uninit(battleProperties);

        return new CharacterTurn(_agentId, false, _hasActed, _initialPosition, _initialDirection);
    }

    private IUiState OnMouseClick(BattleProperties battleProperties)
    {
        IUiState ret = this;

        if (!_hasActed)
        {
            var agent = battleProperties.unitOfWork.AgentRepository.Get(_agentId);
            if (battleProperties.cursor.Selection.Equals(agent.Position))
            {
                IUiState nextState = _hasMoved ? 
                new SelectDirection(_agentId, _initialPosition) : 
                new CharacterTurn(_agentId, false, true, _initialPosition, _initialDirection);

                ret = new SelectAction(_agentId, nextState, OnCancelMove);
            }
            else if (_movablePositions.Contains(battleProperties.cursor.Selection))
            {
                ret = new CharacterMovement(
                    _agentId,
                    battleProperties.cursor.Selection,
                    new SelectAction(
                        _agentId,  
                        new SelectDirection(_agentId, _initialPosition),
                        // _ => new CharacterTurn(_agentId, false, false, _initialPosition, _initialDirection)
                        OnCancelMove
                    )
                );
            }
        }
        else if (_movablePositions.Contains(battleProperties.cursor.Selection))
        {   
            var agent = battleProperties.unitOfWork.AgentRepository.Get(_agentId);    
            ret = battleProperties.cursor.Selection.Equals(agent.Position) ?
            new SelectDirection(_agentId, _initialPosition, OnCancelMove):
            new CharacterMovement(
                _agentId, 
                battleProperties.cursor.Selection, 
                new SelectDirection(_agentId, _initialPosition, OnCancelMove)
            );
        }

        Uninit(battleProperties);

        return ret;
    }
}