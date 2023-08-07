using System;
using System.Collections;

[Serializable]
public class Stats : ValueObject<int[]>
{
    public Stats(
        int strength,
        int defense,
        int magic,
        int magicDefense,
        int agility,
        int accuracy,
        int evasion,
        int luck
    )
    {
        Strength = strength;
        Defense = defense;
        Magic = magic;
        MagicDefense = magicDefense;
        Agility = agility;
        Accuracy = accuracy;
        Evasion = evasion;
        Luck = luck;
    }

    public override int[] Value()
    {
        return new int[] {
            Strength,
            Defense,
            Magic,
            MagicDefense,
            Agility,
            Accuracy,
            Evasion,
            Luck
        };
    }

    public int Strength { get; private set; }
    public int Defense { get; private set; }
    public int Magic { get; private set; }
    public int MagicDefense { get; private set; }
    public int Agility { get; private set; }
    public int Accuracy { get; private set; }
    public int Evasion { get; private set; }
    public int Luck { get; private set; }
}