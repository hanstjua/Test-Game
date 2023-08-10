using System;
using System.Collections.Generic;
using Battle;
using Battle.Common;
using Battle.Common.Armours;
using Battle.Common.Weapons;

public interface ICharacter
{
    public List<Battle.Action> Actions { get; }
    public Stats Stats { get; }
    public int Hp { get; }
    public int Mp { get; }
    public Dictionary<Item, int> Items { get; }
    public int Movements { get; }
    public IWeapon Weapon { get; }
    public IArmour Armour { get; }
}