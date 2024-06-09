using System;
using System.Collections.Generic;

public class CharacterFactory
{
    private static readonly Dictionary<string, ICharacter> _registry = new()
    {
        {"Goblin", new Goblin()}
    };

    public static ICharacter GetGeneric(string name)
    {
        _registry.TryGetValue(name, out ICharacter value);

        if (value == null)
        {
            throw new InvalidOperationException(string.Format("Invalid generic character name {0}.", name));
        }

        return value;
    }
}