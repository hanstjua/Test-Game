using System;
using Battle;
using UnityEngine;

public class SelectDirection : IUiState
{
    private AgentId _agentId;
    private Direction _currentDirection;
    private bool _hasInit = false;
    private Position _prevPosition;
    private Func<BattleProperties, IUiState> _onCancel;

    public SelectDirection(AgentId agentId, Position prevPosition = null, Func<BattleProperties, IUiState> onCancel = null)
    {
        _agentId = agentId;
        _prevPosition = prevPosition;
        _onCancel = onCancel;
    }

    private void Init(BattleProperties battleProperties)
    {
        var agent = battleProperties.unitOfWork.AgentRepository.Get(_agentId);
        _currentDirection = agent.Direction;

        var map = battleProperties.map;
        map.Highlight(map.ToUIPosition(agent.Position));

        _hasInit = true;
    }

    private void Uninit(BattleProperties battleProperties)
    {
        battleProperties.map.ClearHighlights();

        _hasInit = false;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit) Init(battleProperties);

        IUiState ret = this;
        if (Input.GetKey(KeyCode.Return)) ret = OnKeyPress(KeyCode.Return, battleProperties);
        else if (Input.GetKey(KeyCode.UpArrow)) ret = OnKeyPress(KeyCode.UpArrow, battleProperties);
        else if (Input.GetKey(KeyCode.DownArrow)) ret = OnKeyPress(KeyCode.DownArrow, battleProperties);
        else if (Input.GetKey(KeyCode.LeftArrow)) ret = OnKeyPress(KeyCode.LeftArrow, battleProperties);
        else if (Input.GetKey(KeyCode.RightArrow)) ret = OnKeyPress(KeyCode.RightArrow, battleProperties);
        else if (Input.GetKey(KeyCode.Escape)) ret = OnKeyPress(KeyCode.Escape, battleProperties);

        return ret;
    }

    public IUiState OnKeyPress(KeyCode key, BattleProperties battleProperties)
    {
        switch (key)
        {
            case KeyCode.Return:
            var agent = battleProperties.unitOfWork.AgentRepository.Get(_agentId);

            using (var unitOfWork = battleProperties.unitOfWork)
            {
                agent.Face(_currentDirection);
                unitOfWork.AgentRepository.Update(_agentId, agent); 

                unitOfWork.Save();
            }

            float eulerAngle;
            switch (_currentDirection)
            {
                case Direction.North:
                eulerAngle = 0;
                break;

                case Direction.South:
                eulerAngle = 180;
                break;

                case Direction.East:
                eulerAngle = 90;
                break;

                case Direction.West:
                eulerAngle = -90;
                break;

                default:
                eulerAngle = 0;
                break;
            }
            
            battleProperties.characters[_agentId].transform.eulerAngles = new Vector3(0, eulerAngle, 0);

            Uninit(battleProperties);

            return new TransitionBattlePhase();  // return transition state
            
            case KeyCode.UpArrow:
            _currentDirection = Direction.North;
            battleProperties.characters[_agentId].transform.eulerAngles = new Vector3(0, 0, 0);
            return this;

            case KeyCode.DownArrow:
            _currentDirection = Direction.South;
            battleProperties.characters[_agentId].transform.eulerAngles = new Vector3(0, 180, 0);
            return this;

            case KeyCode.LeftArrow:
            _currentDirection = Direction.West;
            battleProperties.characters[_agentId].transform.eulerAngles = new Vector3(0, -90, 0);
            return this;

            case KeyCode.RightArrow:
            _currentDirection = Direction.East;
            battleProperties.characters[_agentId].transform.eulerAngles = new Vector3(0, 90, 0);
            return this;

            case KeyCode.Escape:
            if (_onCancel == null)
            {
                return this;
            }
            else
            {
                battleProperties.characters[_agentId].GetComponent<Character>().SetPosition(_agentId, _prevPosition);
                return _onCancel(battleProperties);
            }

            default:
            return this;
        }
    }
}