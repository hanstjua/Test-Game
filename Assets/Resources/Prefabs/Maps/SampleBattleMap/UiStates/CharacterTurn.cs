using Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CharacterTurn : IUiState
{
    private static ActionDispatcher _actionDispatcher = new ActionDispatcher();
    private Agent _agent;
    private string _chosenAction;

    public CharacterTurn(Agent agent)
    {
        _agent = agent;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        var actionPanel = battleProperties.uiObjects.transform.Find("CameraCanvas/RawImage/ActionsPanel");
        actionPanel.gameObject.SetActive(true);

        if (actionPanel.childCount == 0)
        {
            var buttonObj = GameObject.Find("Button");
            foreach(var action in _agent.Actions)
            {
                var obj = GameObject.Instantiate(buttonObj, actionPanel);
                var text = obj.transform.GetComponentInChildren<Text>();

                text.text = action.Name;

                // make UI Element call ActionDispatcher.Dispatch() on click.
                var button = obj.GetComponent<Button>();
                button.onClick.AddListener(() => _chosenAction = action.Name);
            }
        }

        if (_chosenAction != null)
        {
            for (int i = 0; i < actionPanel.childCount; i++)
            {
                GameObject.Destroy(actionPanel.GetChild(i).gameObject);
            }
            
            actionPanel.gameObject.SetActive(false);

            var handler = _actionDispatcher.Dispatch(_chosenAction);
            return handler.Handle(battleProperties, this);
        }
        else
        {
            return this;
        }
    }
}