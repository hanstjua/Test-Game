using System;
using System.Collections.Generic;


public class ActionDispatcher
{
    private Dictionary<string, IActionHandler> _registry = new Dictionary<string, IActionHandler>{
        {"Attack", new AttackHandler()}
    };

    public IActionHandler Dispatch(string action)
    {
        return _registry[action];
    }
}