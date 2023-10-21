using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Battle.Common;
using Battle.Common.Armours;
using Battle.Common.Weapons;
using Battle.Statuses;


namespace Battle
{
    [Serializable]
    public class Agent : Entity
    {
        private AgentId _id;
        private Position _position;
        private List<Action> _actions;
        private Dictionary<Item, int> _items;
        private HashSet<Status> _statuses;

        public Agent(
            AgentId agentId,
            string name,
            List<Action> actions,
            Stats stats,
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
            Actions = actions;
            Stats = stats;
            Hp = stats.MaxHp;
            Mp = stats.MaxMp;
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

        public string Name { get; private set; }
        public List<Action> Actions 
        { 
            get => new(_actions); 
            private set => _actions = value; 
        }
        public Stats Stats { get; private set; }
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

        public Agent StockItem(Item item)
        {
            if (Items.ContainsKey(item))
            {
                if (Items[item] < 99)
                {
                    Items[item] += 1;
                }
            }
            else
            {
                Items[item] = 1;
            }

            return this;
        }

        public bool IsAlive()
        {
            return Hp > 0;
        }

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

        public bool HasStatus(Type status)
        {
            return Statuses.Any(s => s.GetType() == status);
        }

        public double TurnGaugeIncrement
        {
            get { return (double) Stats.Agility; }
        }

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

        public Agent UpdateStats(Stats stats)
        {
            Stats = stats;

            return this;
        }
    }
}