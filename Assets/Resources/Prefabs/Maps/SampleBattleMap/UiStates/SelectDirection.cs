using System;
using Battle;
using UnityEngine;

public class SelectDirection : IUiState
{
    private Agent _agent;
    private Direction _currentDirection;

    public SelectDirection(Agent agent)
    {
        _agent = agent;
        _currentDirection = agent.Direction;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        IUiState ret = this;
        if (Input.GetKey(KeyCode.Return)) ret = OnKeyPress(KeyCode.Return, battleProperties);
        else if (Input.GetKey(KeyCode.UpArrow)) ret = OnKeyPress(KeyCode.UpArrow, battleProperties);
        else if (Input.GetKey(KeyCode.DownArrow)) ret = OnKeyPress(KeyCode.DownArrow, battleProperties);
        else if (Input.GetKey(KeyCode.LeftArrow)) ret = OnKeyPress(KeyCode.LeftArrow, battleProperties);
        else if (Input.GetKey(KeyCode.RightArrow)) ret = OnKeyPress(KeyCode.RightArrow, battleProperties);

        return ret;
    }

    public IUiState OnKeyPress(KeyCode key, BattleProperties battleProperties)
    {
        switch (key)
        {
            case KeyCode.Return:
            var unitOfWork = battleProperties.unitOfWork;
            var agentId = _agent.Id() as AgentId;

            using(unitOfWork)
            {
                _agent.Face(_currentDirection);
                unitOfWork.AgentRepository.Update(agentId, _agent); 
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
                eulerAngle = -90;
                break;

                case Direction.West:
                eulerAngle = 90;
                break;

                default:
                eulerAngle = 0;
                break;
            }
            
            battleProperties.characters[agentId].transform.eulerAngles = new Vector3(0, eulerAngle, 0);

            return new TransitionBattlePhase();  // return transition state
            
            case KeyCode.UpArrow:
            _currentDirection = Direction.North;
            return this;

            case KeyCode.DownArrow:
            _currentDirection = Direction.South;
            return this;

            case KeyCode.LeftArrow:
            _currentDirection = Direction.West;
            return this;

            case KeyCode.RightArrow:
            _currentDirection = Direction.East;
            return this;

            default:
            return this;
        }
    }
}