using System.Collections.Generic;
using Battle;
using Battle.Common;
using Battle.Common.Armours;
using Battle.Common.Weapons;

public interface ICharacter
{
    public Arbellum[] Arbella { get; }
    public StatLevels Levels { get; }
    public Dictionary<Item, int> Items { get; }
    public int Movements { get; }
    public Weapon Weapon { get; }
    public Armour Armour { get; }
}