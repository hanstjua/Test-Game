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
    private GameObject _directions;
    private Transform _mainCameraTransform;
    private static CameraControl _cameraControl;

    public SelectDirection(AgentId agentId, Position prevPosition = null, Func<BattleProperties, IUiState> onCancel = null)
    {
        _agentId = agentId;
        _prevPosition = prevPosition;
        _onCancel = onCancel;

        _cameraControl = _cameraControl == null ? Camera.main.GetComponent<CameraControl>() : _cameraControl;
        _mainCameraTransform = _cameraControl.transform;
    }

    private void Init(BattleProperties battleProperties)
    {
        var agent = battleProperties.unitOfWork.AgentRepository.Get(_agentId);
        _currentDirection = agent.Direction;

        var map = battleProperties.map;
        map.Highlight(map.ToUIPosition(agent.Position));

        _directions = Directions.Create("Prefabs/Maps/Directions", map.ToUIPosition(agent.Position), battleProperties.characters[_agentId].transform.rotation);

        _cameraControl.FocusAt(battleProperties.map.ToUIPosition(agent.Position));

        _hasInit = true;
    }
 
    private void Uninit(BattleProperties battleProperties)
    {
        battleProperties.map.ClearHighlights();

        GameObject.Destroy(_directions);

        _hasInit = false;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit) Init(battleProperties);

        _cameraControl.HandleCameraInput();

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
        Vector3 eulerAngles;

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

            var eulerAngle = _currentDirection switch
            {
                Direction.North => 0,
                Direction.South => 180,
                Direction.East => 90,
                Direction.West => -90,
                _ => (float)0,
            };
            battleProperties.characters[_agentId].transform.eulerAngles = new Vector3(0, eulerAngle, 0);

            Uninit(battleProperties);

            return new TransitionBattlePhase();  // return transition state
            
            case KeyCode.UpArrow:
            eulerAngles = new Vector3(0, _mainCameraTransform.eulerAngles.y - 45.0f, 0);
            _currentDirection = AngleToDirection(eulerAngles.y);
            battleProperties.characters[_agentId].transform.eulerAngles = eulerAngles;
            _directions.transform.eulerAngles = eulerAngles;

            return this;

            case KeyCode.DownArrow:
            eulerAngles = new Vector3(0, _mainCameraTransform.eulerAngles.y - 225.0f, 0);
            _currentDirection = AngleToDirection(eulerAngles.y);
            battleProperties.characters[_agentId].transform.eulerAngles = eulerAngles;
            _directions.transform.eulerAngles = eulerAngles;

            return this;

            case KeyCode.LeftArrow:
            eulerAngles = new Vector3(0, _mainCameraTransform.eulerAngles.y - 135.0f, 0);
            _currentDirection = AngleToDirection(eulerAngles.y);
            battleProperties.characters[_agentId].transform.eulerAngles = eulerAngles;
            _directions.transform.eulerAngles = eulerAngles;

            return this;

            case KeyCode.RightArrow:
            eulerAngles = new Vector3(0, _mainCameraTransform.eulerAngles.y + 45.0f, 0);
            _currentDirection = AngleToDirection(eulerAngles.y);
            battleProperties.characters[_agentId].transform.eulerAngles = eulerAngles;
            _directions.transform.eulerAngles = eulerAngles;

            return this;

            case KeyCode.Escape:
            if (_onCancel == null)
            {
                return this;
            }
            else
            {
                Uninit(battleProperties);
                return _onCancel(battleProperties);
            }

            default:
            return this;
        }
    }

    Direction AngleToDirection(float angle)
    {
        var threshold = 0.1f;

        if (Math.Abs(angle) <= threshold || Math.Abs(angle - 360f) <= threshold) return Direction.North;
        else if (Math.Abs(angle - 90f) <= threshold || Math.Abs(angle + 270f) <= threshold) return Direction.East;
        else if (Math.Abs(angle - 180f) <= threshold || Math.Abs(angle + 180f) <= threshold) return Direction.South;
        else return Direction.West;
    }
}