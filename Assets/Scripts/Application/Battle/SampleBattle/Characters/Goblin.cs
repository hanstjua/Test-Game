using System.Collections.Generic;
using Battle;
using Battle.Accessories;
using Battle.Armours;
using Battle.Footwears;
using Battle.Weapons;
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
    public Armour Armour => new LeatherArmour();
    public Arbellum[] Arbella => new Arbellum[] {new Physical(0, true)};

    public Handheld RightHand => new Longsword();

    public Handheld LeftHand => null;

    public Footwear Footwear => null;

    public Accessory Accessory1 => null;

    public Accessory Accessory2 => null;
}