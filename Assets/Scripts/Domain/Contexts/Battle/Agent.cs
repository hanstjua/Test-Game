using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Common;
using Battle.Common.Armours;
using Battle.Common.Weapons;
using Battle.Statuses;

namespace Battle
{
    [Serializable]
    public class Agent : Entity
    {
        private readonly AgentId _id;
        private Position _position;
        private List<Action> _actions;
        private Dictionary<Item, int> _items;
        private HashSet<Status> _statuses;
        private Stats _augmentation = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        public Agent(
            AgentId agentId,
            string name,
            Arbellum[] arbella,
            StatLevels statLevels,
            Position position,
            Dictionary<Item, int> items,
            int movements,
            Weapon weapon,
            Armour armour,
            double turnGauge = 0.0,
            Direction direction = Direction.North
        )
        {
            _id = agentId;
            Name = name;
            Arbella = arbella;
            StatLevels = statLevels;
            Hp = Stats.MaxHp;
            Mp = Stats.MaxMp;
            Position = position;
            Items = items;
            Statuses = new HashSet<Status>();
            TurnGauge = turnGauge;
            Movements = movements;
            Direction = direction;
            Weapon = weapon;
            Armour = armour;
        }

        public override object Id() {
            return _id;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name;
        public Arbellum[] Arbella { get; private set; }
        public Action[] Actions 
        { 
            get => Arbella.SelectMany(a => a.Actives).ToArray();
        }
        public StatLevels StatLevels { get; private set; }
        public Stats Stats 
        {
            get => StatLevels.ToStats().Augment(_augmentation);
        }
        public int Hp { get; private set; }
        public int Mp { get; private set; }
        public Position Position 
        { 
            get => _position; 
            set => _position = value; 
        }
        public Dictionary<Item, int> Items 
        { 
            get => new(_items);
            private set => _items = value;
        }
        public HashSet<Status> Statuses
        { 
            get => new(_statuses);
            private set => _statuses = value;
        }
        public double TurnGauge { get; private set; }
        public int Movements { get; private set; }
        public Direction Direction { get; private set; }
        public Weapon Weapon { get; private set; }
        public Armour Armour { get; private set; }

        public Agent ReduceHp(int damage)
        {
            Hp = damage > Hp ? 0 : Hp - damage;
            return this;
        }

        public Agent RestoreHp(int gain)
        {
            Hp += gain;
            return this;
        }

        public Agent ReduceMp(int damage)
        {
            Mp = damage > Mp ? 0 : Mp - damage;
            return this;
        }

        public Agent RestoreMp(int gain)
        {
            Mp += gain;
            return this;
        }

        public Agent Move(Position to)
        {
            if (Position.MovementCost(to) > Movements)
            {
                throw new InvalidOperationException(String.Format("({0}, {1}, {2}) is out of movement range!", to.X, to.Y, to.Z));
            }

            Position = to;

            return this;
        }

        public Agent Face(Direction direction)
        {
            Direction = direction;

            return this;
        }

        public Agent ConsumeItem(Item item)
        {
            if (!Items.ContainsKey(item))
            {
                throw new InvalidOperationException(String.Format("Agent doesn't have any {0}!", Enum.GetName(typeof(Item), item)));
            }
            else
            {
                if (Items[item] == 1)
                {
                    Items.Remove(item);
                }
                else
                {
                    Items[item] -= 1;
                }
            }

            return this;
        }

        public Agent StockItem(Item item, int amount)
        {
            Items[item] = Items.ContainsKey(item) ? Math.Min(Items[item] + amount, 99) : Math.Min(amount, 99);

            return this;
        }

        public bool IsAlive() => Hp > 0;

        public Agent AddStatus(Status status)
        {
            _statuses.Add(status);

            return this;
        }

        public Agent RemoveStatus(Status status)
        {
            _statuses.Remove(status);

            return this;
        }

        public bool HasStatus(Type status) =>  Statuses.Any(s => s.GetType() == status);

        public double TurnGaugeIncrement => Stats.Agility;

        public Agent RaiseTurnGauge()
        {
            TurnGauge += TurnGaugeIncrement;

            return this;
        }

        public Agent ConsumeTurnGauge()
        {
            if (TurnGauge >= 100)
            {
                TurnGauge -= 100;
            }

            return this;
        }

        public Agent ResetTurnGauge()
        {
            TurnGauge = 0;

            return this;
        }

        public Agent IncreaseStats(Dictionary<StatType, int> increments)
        {
            _augmentation = increments.Aggregate(_augmentation, (stats, inc) => stats.ModifyStat(inc.Key, inc.Value));

            return this;
        }

        public Agent LevelsUp(Dictionary<StatType, uint> increments)
        {
            StatLevels = increments.Aggregate(StatLevels, (levels, inc) => levels.IncreaseLevel(inc.Key, inc.Value));

            return this;
        }

        public Agent ArbellumUp(ArbellumType type, int points)
        {
            Arbella.First(a => a.Type == type).ExperienceUp(points);

            return this;
        }
    }
}