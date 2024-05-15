using System.Collections.Generic;
using Battle;
using Battle.Common;
using Battle.Common.Armours;
using Battle.Common.Weapons;
using Battle.Services.Actions;
using Battle.Services.Arbella;

public class Goblin : ICharacter
{
    public StatLevels Levels => new(
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        100,
        300,
        100
    );

    public Dictionary<Item, int> Items => new();
    public int Movements => 2;
    public Weapon Weapon => new Longsword();
    public Armour Armour => new LeatherArmour();
    public Arbellum[] Arbella => new Arbellum[] {new Physical(0)};
}