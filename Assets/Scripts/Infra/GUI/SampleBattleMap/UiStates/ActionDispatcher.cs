using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


public class ActionDispatcher
{
    private static Dictionary<string, Battle.Action> _registry = null;

    public Battle.Action Dispatch(string action)
    {
        _registry ??= Assembly
        .GetAssembly(typeof(Battle.Action))
        .GetTypes()
        .Where(t => t.IsSubclassOf(typeof(Battle.Action)))
        .Select(t => (Battle.Action) Activator.CreateInstance(t))
        .ToDictionary(a => a.Name, a => a);

        return _registry[action];
    }
}