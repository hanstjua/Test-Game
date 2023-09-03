using System;
using System.Collections.Generic;


public class ActionDispatcher
{
    private Dictionary<string, ActionHandler> _registry = new Dictionary<string, ActionHandler>{
        {"Attack", new AttackHandler()},
        {"Defend", new DefendHandler()},
        {"End Turn", new EndTurnHandler()}
    };

    public ActionHandler Dispatch(string action)
    {
        return _registry[action];
    }
}