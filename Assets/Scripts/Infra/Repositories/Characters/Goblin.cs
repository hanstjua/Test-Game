using System.Collections.Generic;
using Battle;
using Battle.Common;
using Battle.Common.Armours;
using Battle.Common.Weapons;
using Battle.Services.Actions;

public class Goblin : ICharacter
{
    public List<Battle.Action> Actions => new List<Battle.Action>
    { 
        new Attack(), 
        new Defend()
    };
    public Stats Stats => new Stats(
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

    public Dictionary<Item, int> Items => new Dictionary<Item, int>();
    public int Movements => 2;
    public Weapon Weapon => new Longsword();
    public Armour Armour => new LeatherArmour();
}