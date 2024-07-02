using System;

namespace Battle
{   
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

        public Stats ModifyStat(StatType type, int value)
        {
            switch (type)
            {
                case StatType.Strength:
                Strength += value < 0 ? 0 : value;
                break;

                case StatType.Defense:
                Defense += value < 0 ? 0 : value;
                break;

                case StatType.Magic:
                Magic += value < 0 ? 0 : value;
                break;

                case StatType.MagicDefense:
                MagicDefense += value < 0 ? 0 : value;
                break;

                case StatType.Agility:
                Agility += value < 0 ? 0 : value;
                break;

                case StatType.Accuracy:
                Accuracy += value < 0 ? 0 : value;
                break;

                case StatType.Evasion:
                Evasion += value < 0 ? 0 : value;
                break;

                case StatType.Luck:
                Luck += value < 0 ? 0 : value;
                break;

                case StatType.MaxHp:
                MaxHp += value < 0 ? 0 : value;
                break;

                case StatType.MaxMp:
                MaxMp += value < 0 ? 0 : value;
                break;
            }

            return this;
        }

        public Stats Augment(Stats augmentation)
        {
            return new(
                Strength + augmentation.Strength,
                Defense + augmentation.Defense,
                Magic + augmentation.Magic,
                MagicDefense + augmentation.MagicDefense,
                Agility + augmentation.Agility,
                Accuracy + augmentation.Accuracy,
                Evasion + augmentation.Evasion,
                Luck + augmentation.Luck,
                MaxHp + augmentation.MaxHp,
                MaxMp + augmentation.MaxMp
            );
        }
    }
}