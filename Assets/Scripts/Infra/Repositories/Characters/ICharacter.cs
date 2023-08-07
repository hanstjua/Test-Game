using System;
using System.Collections.Generic;
using Battle;
using Battle.Common;

public interface ICharacter
{
    public List<Battle.Action> Actions { get; }
    public Stats Stats { get; }
    public int Hp { get; }
    public int Mp { get; }
    public Dictionary<Item, int> Items { get; }
    public int Movements { get; }
}