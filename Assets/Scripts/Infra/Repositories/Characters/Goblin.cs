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
        1
    );
    public int Hp => 3;
    public int Mp => 0;
    public Dictionary<Item, int> Items => new Dictionary<Item, int>();
    public int Movements => 3;
    public IWeapon Weapon => new Longsword();
    public IArmour Armour => new LeatherArmour();
}