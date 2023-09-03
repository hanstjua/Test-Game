using System;
using System.Collections;

[Serializable]
public class Stats : ValueObject<int[]>
{
    public enum Type
    {
        Strength,
        Defense,
        Magic,
        MagicDefense,
        Agility,
        Accuracy,
        Evasion,
        Luck,
        MaxHp,
        MaxMp
    }

    public Stats(
        int strength,
        int defense,
        int magic,
        int magicDefense,
        int agility,
        int accuracy,
        int evasion,
        int luck,
        int maxHp,
        int maxMp
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
        MaxHp = maxHp;
        MaxMp = maxMp;
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
            Luck,
            MaxHp,
            MaxMp
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
    public int MaxHp { get; private set; }
    public int MaxMp { get; private set; }

    public Stats ModifyStat(Type type, int value)
    {
        switch (type)
        {
            case Type.Strength:
            Strength = value;
            break;

            case Type.Defense:
            Defense = value;
            break;

            case Type.Magic:
            Magic = value;
            break;

            case Type.MagicDefense:
            MagicDefense = value;
            break;

            case Type.Agility:
            Agility =  value;
            break;

            case Type.Accuracy:
            Accuracy = value;
            break;

            case Type.Evasion:
            Evasion = value;
            break;

            case Type.Luck:
            Luck = value;
            break;

            case Type.MaxHp:
            MaxHp = value < 0 ? 0 : value;
            break;

            case Type.MaxMp:
            MaxMp = value < 0 ? 0 : value;
            break;
        }

        return this;
    }
}