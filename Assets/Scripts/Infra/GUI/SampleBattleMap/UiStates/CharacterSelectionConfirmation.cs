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
    private static ConfirmCharacters _confirmSelectionPanel;

    public CharacterSelectionConfirmation(GameObject uiObjects, IUiState previousState)
    {
        _previousState = previousState;
        _uiObjects = uiObjects;        
    }

    private void Init()
    {
        if (_confirmSelectionPanel == null) _confirmSelectionPanel = _uiObjects.transform.Find("CameraCanvas/RawImage/ConfirmCharacters").GetComponent<ConfirmCharacters>();

        _confirmSelectionPanel.Show();

        _confirmSelectionPanel.Yes.AddListener(() => _isConfirm = 1);
        _confirmSelectionPanel.No.AddListener(() => _isConfirm = 0);

        _hasInit = true;
    }

    private void Uninit(BattleProperties battleProperties)
    {
        _confirmSelectionPanel.Hide();

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