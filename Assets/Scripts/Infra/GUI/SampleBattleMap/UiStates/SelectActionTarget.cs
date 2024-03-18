using System.Linq;
using Battle;
using UnityEngine;
using UnityEngine.Events;


public class SelectActionTarget : IUiState
{
    private Action _action;
    private IUiState _onCancel;
    private IUiState _onProceed;
    private bool _hasInit = false;
    private UnityAction<Position, Position> _characterPanelUpdater;

    public SelectActionTarget(Action action, IUiState onCancel, IUiState onProceed)
    {
        _action = action;
        _onCancel = onCancel;
        _onProceed = onProceed;
    }

    private void Init(BattleProperties battleProperties)
    {
        var characterPanel = battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/CharacterPanel").GetComponent<CharacterPanel>();
        _characterPanelUpdater = new((_, newPos) => characterPanel.UpdateCharacterPanelByPosition(battleProperties, newPos));
        battleProperties.battleEvents.cursorSelectionChanged.AddListener(_characterPanelUpdater);

        // highlight map
        var map = battleProperties.map;
        var battle = battleProperties.unitOfWork.BattleRepository.Get(battleProperties.battleId);
        var battleField = battleProperties.unitOfWork.BattleFieldRepository.Get(battle.BattleFieldId);
        var actorPosition = battleProperties.unitOfWork.AgentRepository.Get(battle.ActiveAgent).Position;
        var positionsToHighlight = _action.TargetArea.RelativePositions
        .Select(p => actorPosition.TranslateBy(p))
        .Where(p => p.X >= 0 && p.Y >= 0 && battleField.Terrains[p.X][p.Y].Traversable);
        var uiPositionsToHighlight = positionsToHighlight.Select(p => map.ToUIPosition(p));
        
        foreach(var position in uiPositionsToHighlight)
        {
            map.Highlight(position);
        }

        _hasInit = true;
    }

    private void Uninit(BattleProperties battleProperties)
    {
        battleProperties.battleEvents.cursorSelectionChanged.RemoveListener(_characterPanelUpdater);

        battleProperties.cursor.Reset();

        // clear map highlights
        battleProperties.map.ClearHighlights();

        _hasInit = false;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit) Init(battleProperties);

        Camera.main.GetComponent<CameraControl>().HandleCameraInput();

        battleProperties.cursor.UpdateSelection();

        IUiState ret = this;
        if (Input.GetMouseButtonDown(0))
        {
            var position = battleProperties.cursor.Selection;
            var battle = battleProperties.unitOfWork.BattleRepository.Get(battleProperties.battleId);
            var actor = battleProperties.unitOfWork.AgentRepository.Get(battle.ActiveAgent);
            var targets = battleProperties.unitOfWork.AgentRepository
            .GetAll()
            .Where(
                a => (a.Position.Equals(position) || _action.AreaOfEffect.IsWithin(actor.Position, a.Position)) 
                && _action.IsTargetValid(a, actor)
            )
            .ToArray();

            if (targets.Length > 0)
            {
                Uninit(battleProperties);
                var outcomes = _action.Execute(actor, targets, battle, battleProperties.unitOfWork);
                ret = new ActionTriggeredExecution(
                    actor,
                    _action.ToString(),
                    ExecutionStateDispatcher.Dispatch(outcomes, _onProceed)
                );
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Uninit(battleProperties);
            ret = _onCancel;
        }

        return ret;
    }
}