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
    private static GameObject _confirmSelectionPanel;

    public CharacterSelectionConfirmation(GameObject uiObjects, IUiState previousState)
    {
        _previousState = previousState;

        if (_confirmSelectionPanel == null) _confirmSelectionPanel = uiObjects.transform.Find("CameraCanvas/RawImage/ConfirmSelectionPanel").gameObject;

        _confirmSelectionPanel.SetActive(true);

        _confirmSelectionPanel.transform.Find("Yes").GetComponent<Button>().onClick.AddListener(() => _isConfirm = 1);
        _confirmSelectionPanel.transform.Find("No").GetComponent<Button>().onClick.AddListener(() => _isConfirm = 0);
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        switch (_isConfirm)
        {
            case 1:
            Debug.Log("yes");
            _confirmSelectionPanel.SetActive(false);
            return new BattleCommencement(battleProperties.uiObjects);

            case 0:
            Debug.Log("no");
            _confirmSelectionPanel.SetActive(false);
            return _previousState;

            default:
            return this;
        }
    }
}