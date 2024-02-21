using System.Collections.Generic;
using Battle;
using Battle.Common;
using Battle.Common.Armours;
using Battle.Common.Weapons;
using Battle.Services.Actions;

public class Goblin : ICharacter
{
    public List<Action> Actions => new()
    { 
        new Attack(), 
        new Defend()
    };
    public Stats Stats => new(
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        1,
        3,
        1
    );

    public Dictionary<Item, int> Items => new();
    public int Movements => 2;
    public Weapon Weapon => new Longsword();
    public Armour Armour => new LeatherArmour();
}