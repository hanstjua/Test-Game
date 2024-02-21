using System;
using System.Collections.Generic;


public class ActionDispatcher
{
    private readonly Dictionary<string, ActionHandler> _registry = new()
    {
        {"Attack", new AttackHandler()},
        {"Defend", new DefendHandler()},
        {"End Turn", new EndTurnHandler()}
    };

    public ActionHandler Dispatch(string action)
    {
        return _registry[action];
    }
}