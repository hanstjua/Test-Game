using System;
using Battle;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using System.Linq;

public class CharacterMovement : IUiState
{
    const double TOLERANCE = 0.01;
    const float SPEED = 8;

    private AgentId _agentId;
    private Position _endPosition;
    private IUiState _nextState;
    private bool _hasInit = false;
    private Position[] _path;
    private Position _currentPosition;
    private Position _nextPosition;

    public CharacterMovement(AgentId agentId, Position to, IUiState nextState)
    {
        _agentId = agentId;
        _endPosition = to;
        _nextState = nextState;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit)
        {
            using (var unitOfWork = battleProperties.unitOfWork)
            {
                var agent = unitOfWork.AgentRepository.Get(_agentId);
                var battle = unitOfWork.BattleRepository.Get(battleProperties.battleId);
                var fieldId = battle.BattleFieldId;

                var adversaryIds = battle.PlayerIds.Contains(_agentId) ? battle.EnemyIds : battle.PlayerIds;
                var adversaryPositions = adversaryIds.Select(id => unitOfWork.AgentRepository.Get(id).Position).ToArray();

                _path = new GetMovePathService().Execute(agent.Position, _endPosition, unitOfWork.BattleFieldRepository.Get(fieldId), adversaryPositions);

                _currentPosition = agent.Position;
                _nextPosition = _path[0];

                agent = agent.Move(_endPosition);
                unitOfWork.AgentRepository.Update(_agentId, agent);

                unitOfWork.Save();
            }

            _hasInit = true;
        }

        var character = battleProperties.characters[_agentId];
        var nextPosition = battleProperties.map.ToUIPosition(_path[0]) + Character.POSITION_OFFSET;

        IUiState ret = this;

        if (hasReached(character.transform.position, nextPosition))
        {
            if (_path[0].Equals(_endPosition))  // move completed
            {
                ret = _nextState;
            }
            else
            {
                _currentPosition = _path[0];
                _nextPosition = _path[1];

                _path = _path.Skip(1).ToArray();  // update path
            }
        }
        else
        {
            character.transform.position = Vector3.MoveTowards(character.transform.position, nextPosition, SPEED * Time.deltaTime);  // move in progress

            var deltaPosition = _nextPosition.RelativeTo(_currentPosition);
            character.transform.eulerAngles = new Vector3(0, (deltaPosition.X * 90) + (deltaPosition.Y * (1 - deltaPosition.Y) * 90), 0);
        }

        return ret;
    }

    private bool hasReached(Vector3 current, Vector3 destination)
    {
        return Vector3.Distance(current, destination) <= TOLERANCE;
    }
}