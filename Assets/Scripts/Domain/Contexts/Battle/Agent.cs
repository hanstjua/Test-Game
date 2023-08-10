using System;
using System.Collections;
using System.Collections.Generic;
using Battle.Common;
using Battle.Common.Armours;
using Battle.Common.Weapons;
using Codice.Client.Common.WebApi;

namespace Battle
{
    [Serializable]
    public class Agent : Entity
    {
        private AgentId _id;
        private Position _position;

        public Agent(
            AgentId agentId,
            string name,
            List<Action> actions,
            Stats stats,
            int hp,
            int mp,
            Position position,
            Dictionary<Item, int> items,
            int movements,
            IWeapon weapon,
            IArmour armour,
            double turnGauge = 0.0,
            Direction direction = Direction.North
        )
        {
            _id = agentId;
            Name = name;
            Actions = actions;
            Stats = stats;
            Hp = hp;
            Mp = mp;
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
        public List<Action> Actions { get; private set; }
        private Stats Stats { get; set; }
        public int Hp { get; private set; }
        public int Mp { get; private set; }
        public Position Position { get => _position; set => _position = value; }
        private Dictionary<Item, int> Items { get; set; }
        private HashSet<Status> Statuses { get; set; }
        public double TurnGauge { get; private set; }
        public int Movements { get; private set; }
        public Direction Direction { get; private set; }
        public IWeapon Weapon { get; private set; }
        public IArmour Armour { get; private set; }

        public int GetStrength() { return Stats.Strength; }
        public int GetDefense() { return Stats.Defense; }
        public int GetMagic() { return Stats.Magic; }
        public int GetMagicDefense() { return Stats.MagicDefense; }
        public int GetAgility() { return Stats.Agility; }
        public int GetAccuracy() { return Stats.Accuracy; }
        public int GetEvasion() { return Stats.Evasion; }
        public int GetLuck() { return Stats.Luck; }
        public int GetX() { return Position.X; }
        public int GetY() { return Position.Y; }
        public int GetZ() { return Position.Z; }

        public Agent ReduceHp(int damage)
        {
            Hp -= damage;
            return this;
        }

        public Agent RestoreHp(int gain)
        {
            Hp += gain;
            return this;
        }

        public Agent ReduceMp(int damage)
        {
            Mp -= damage;
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
            Statuses.Add(status);

            return this;
        }

        public Agent RemoveStatus(Status status)
        {
            Statuses.Remove(status);

            return this;
        }

        public bool HasStatus(Status status)
        {
            return Statuses.Contains(status);
        }

        public double TurnGaugeIncrement
        {
            get { return (double) Stats.Agility; }
        }

        public void RaiseTurnGauge()
        {
            TurnGauge += TurnGaugeIncrement;
        }

        public void ResetTurnGauge()
        {
            if (TurnGauge >= 100)
            {
                TurnGauge -= 100;
            }
        }
    }
}