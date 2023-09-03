using Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class CharacterSelectionConfirmation : IUiState
{
    private int _isConfirm = -1;
    private IUiState _previousState;
    private GameObject _uiObjects;
    private bool _hasInit = false;
    private static GameObject _confirmSelectionPanel;

    public CharacterSelectionConfirmation(GameObject uiObjects, IUiState previousState)
    {
        _previousState = previousState;
        _uiObjects = uiObjects;        
    }

    private void Init()
    {
        if (_confirmSelectionPanel == null) _confirmSelectionPanel = _uiObjects.transform.Find("CameraCanvas/RawImage/ConfirmSelectionPanel").gameObject;

        _confirmSelectionPanel.GetComponent<CanvasGroup>().alpha = 1;

        _confirmSelectionPanel.transform.Find("Yes").GetComponent<Button>().onClick.AddListener(() => _isConfirm = 1);
        _confirmSelectionPanel.transform.Find("No").GetComponent<Button>().onClick.AddListener(() => _isConfirm = 0);

        _hasInit = true;
    }

    private void Uninit(BattleProperties battleProperties)
    {
        _confirmSelectionPanel.GetComponent<CanvasGroup>().alpha = 0;

        battleProperties.map.ClearHighlights();

        _hasInit = false;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        if (!_hasInit)
        {
            Init();
        }

        switch (_isConfirm)
        {
            case 1:
            Uninit(battleProperties);
            return new BattleCommencement(battleProperties.uiObjects);

            case 0:
            Uninit(battleProperties);
            return _previousState;

            default:
            return this;
        }
    }
}