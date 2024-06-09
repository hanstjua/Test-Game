using System;

namespace Battle
{    
    [Serializable]
    public class StatLevels : ValueObject<uint[]>
    {
        public const int MAX_LEVEL = 99999;

        public StatLevels(
            uint strength,
            uint defense,
            uint magic,
            uint magicDefense,
            uint agility,
            uint accuracy,
            uint evasion,
            uint luck,
            uint maxHp,
            uint maxMp
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

        public override uint[] Value()
        {
            return new uint[] {
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

        public static StatLevels MaxLevels
        {
            get => new(
                MAX_LEVEL,
                MAX_LEVEL,
                MAX_LEVEL,
                MAX_LEVEL,
                MAX_LEVEL,
                MAX_LEVEL,
                MAX_LEVEL,
                MAX_LEVEL,
                MAX_LEVEL,
                MAX_LEVEL
            );
        }

        public uint Strength { get; private set; }
        public uint Defense { get; private set; }
        public uint Magic { get; private set; }
        public uint MagicDefense { get; private set; }
        public uint Agility { get; private set; }
        public uint Accuracy { get; private set; }
        public uint Evasion { get; private set; }
        public uint Luck { get; private set; }
        public uint MaxHp { get; private set; }
        public uint MaxMp { get; private set; }

        public StatLevels IncreaseLevel(StatType type, uint value)
        {
            switch (type)
            {
                case StatType.Strength:
                Strength = Strength + value < MAX_LEVEL + 1 ? Strength + value : MAX_LEVEL;
                break;

                case StatType.Defense:
                Defense = Defense + value < MAX_LEVEL + 1 ? Defense + value : MAX_LEVEL;;
                break;

                case StatType.Magic:
                Magic = Magic + value < MAX_LEVEL + 1 ? Magic + value : MAX_LEVEL;;
                break;

                case StatType.MagicDefense:
                MagicDefense = MagicDefense + value < MAX_LEVEL + 1 ? MagicDefense + value : MAX_LEVEL;;
                break;

                case StatType.Agility:
                Agility = Agility + value < MAX_LEVEL + 1 ? Agility + value : MAX_LEVEL;;
                break;

                case StatType.Accuracy:
                Accuracy = Accuracy + value < MAX_LEVEL + 1 ? Accuracy + value : MAX_LEVEL;
                break;

                case StatType.Evasion:
                Evasion = Evasion + value < MAX_LEVEL + 1 ? Evasion + value : MAX_LEVEL;
                break;

                case StatType.Luck:
                Luck = Luck + value < MAX_LEVEL + 1 ? Luck + value : MAX_LEVEL;
                break;

                case StatType.MaxHp:
                MaxHp = MaxHp + value < MAX_LEVEL + 1 ? MaxHp + value : MAX_LEVEL;
                break;

                case StatType.MaxMp:
                MaxMp = MaxMp + value < MAX_LEVEL + 1 ? MaxMp + value : MAX_LEVEL;
                break;
            }

            return this;
        }

        public Stats ToStats()
        {
            return new(
                (int) Strength / 100,
                (int) Defense / 100,
                (int) Magic / 100,
                (int) MagicDefense / 100,
                (int) Agility / 100,
                (int) Accuracy / 100,
                (int) Evasion / 100,
                (int) Luck / 100,
                (int) MaxHp / 10,
                (int) MaxMp / 10
            );
        }
    }
}