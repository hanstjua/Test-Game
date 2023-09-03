using System;
using System.Linq;
using Battle;
using UnityEngine;
using UnityEngine.Events;


public class SelectActionTarget : IUiState
{
    private ActionHandler _actionHandler;
    private bool _hasInit = false;
    private UnityAction<Position, Position> _characterPanelUpdater;

    public SelectActionTarget(ActionHandler actionHandler)
    {
        _actionHandler = actionHandler;
    }

    private void Init(BattleProperties battleProperties)
    {
        var characterPanel = battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/CharacterPanel").GetComponent<CharacterPanel>();
        _characterPanelUpdater = new((_, newPos) => characterPanel.UpdateCharacterPanel(battleProperties, newPos));
        battleProperties.battleEvents.cursorSelectionChanged.AddListener(_characterPanelUpdater);

        // highlight map
        var map = battleProperties.map;
        var battle = battleProperties.unitOfWork.BattleRepository.Get(battleProperties.battleId);
        var battleField = battleProperties.unitOfWork.BattleFieldRepository.Get(battle.BattleFieldId);
        var actorPosition = battleProperties.unitOfWork.AgentRepository.Get(battle.ActiveAgent).Position;
        var positionsToHighlight = _actionHandler.Service.AreaOfEffect.RelativePositions
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

        battleProperties.cursor.Selection = new Position(-1, -1, -1);

        // clear map highlights
        battleProperties.map.ClearHighlights();

        _hasInit = false;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit) Init(battleProperties);

        battleProperties.cursor.UpdateSelection();

        IUiState ret = this;
        if (Input.GetMouseButtonDown(0))
        {
            var position = battleProperties.cursor.Selection;
            var agent = battleProperties.unitOfWork.AgentRepository.GetAll().ToList().Find(a => a.Position.Equals(position) && _actionHandler.ValidateTarget(a));
            if (agent != null)
            {
                Uninit(battleProperties);
                ret = _actionHandler.ExecuteAction(agent);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Uninit(battleProperties);
            ret = _actionHandler.CancelAction();
        }

        return ret;
    }
}