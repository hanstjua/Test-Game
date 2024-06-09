using System.Collections.Generic;
using Battle;
using Battle.Accessories;
using Battle.Armours;
using Battle.Footwears;

#nullable enable

public interface ICharacter
{
    public Arbellum[] Arbella { get; }
    public StatLevels Levels { get; }
    public Dictionary<Item, int> Items { get; }
    public int Movements { get; }
    public Handheld? RightHand { get; }
    public Handheld? LeftHand { get; }
    public Armour? Armour { get; }
    public Footwear? Footwear { get; }
    public Accessory? Accessory1 { get; }
    public Accessory? Accessory2 { get; }
}

#nullable disable