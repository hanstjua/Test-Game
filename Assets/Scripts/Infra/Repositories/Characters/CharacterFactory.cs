using System;
using System.Collections.Generic;

public class CharacterFactory
{
    private static Dictionary<string, ICharacter> _registry = new Dictionary<string, ICharacter>()
    {
        {"Goblin", new Goblin()}
    };

    public static ICharacter GetGeneric(string name)
    {
        ICharacter value = null;
        _registry.TryGetValue(name, out value);

        if (value == null)
        {
            throw new InvalidOperationException(String.Format("Invalid generic character name {0}.", name));
        }

        return value;
    }
}