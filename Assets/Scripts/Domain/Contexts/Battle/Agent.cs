using System;
using System.Collections.Generic;
using System.Linq;
using Battle;
using Battle.Accessories;
using Battle.Armours;
using Battle.Footwears;
using Battle.Statuses;

#nullable enable

namespace Battle
{
    [Serializable]
    public class Agent : Entity
    {
        private readonly AgentId _id;
        private Stats _augmentation = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        public Agent(
            AgentId agentId,
            string name,
            Arbellum[] arbella,
            StatLevels statLevels,
            Position position,
            Dictionary<Item, int> items,
            int movements,
            Handheld? rightHand,
            Handheld? leftHand,
            Armour? armour,
            Footwear? footwear,
            Accessory? accessory1,
            Accessory? accessory2,
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
            RightHand = rightHand;
            LeftHand = leftHand;
            Armour = armour;
            Footwear = footwear;
            Accessory1 = accessory1;
            Accessory2 = accessory2;
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
        public Position Position { get; private set; }
        public Dictionary<Item, int> Items { get; private set; }
        public HashSet<Status> Statuses { get; private set; }
        public double TurnGauge { get; private set; }
        public int Movements { get; private set; }
        public Direction Direction { get; private set; }
        public Handheld? RightHand { get; private set; }
        public Handheld? LeftHand { get; private set; }
        public Armour? Armour { get; private set; }
        public Footwear? Footwear { get; private set; }
        public Accessory? Accessory1 { get; private set; }
        public Accessory? Accessory2 { get; private set; }

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

        public Agent Teleport(Position to)
        {
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
            Statuses.Add(status);

            return this;
        }

        public Agent RemoveStatus(Status status)
        {
            Statuses.Remove(status);

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

        public Agent RightHandEquip(Handheld? handheld)
        {
            if (RightHand != null) _augmentation = _augmentation.Deaugment(RightHand.StatsBoost);
            if (handheld != null) _augmentation = _augmentation.Augment(handheld.StatsBoost);

            RightHand = handheld;

            return this;
        }

        public Agent LeftHandEquip(Handheld? handheld)
        {
            if (LeftHand != null) _augmentation = _augmentation.Deaugment(LeftHand.StatsBoost);
            if (handheld != null) _augmentation = _augmentation.Augment(handheld.StatsBoost);

            LeftHand = handheld;

            return this;
        }

        public Agent ArmourEquip(Armour? armour)
        {
            if (Armour != null) _augmentation = _augmentation.Deaugment(Armour.StatsBoost);
            if (armour != null) _augmentation = _augmentation.Augment(armour.StatsBoost);

            Armour = armour;

            return this;
        }

        public Agent FootwearEquip(Footwear? footwear)
        {
            if (Footwear != null) _augmentation = _augmentation.Deaugment(Footwear.StatsBoost);
            if (footwear != null) _augmentation = _augmentation.Augment(footwear.StatsBoost);

            Footwear = footwear;

            return this;
        }

        public Agent Accessory1Equip(Accessory? accessory)
        {
            if (Accessory1 != null) _augmentation = _augmentation.Deaugment(Accessory1.StatsBoost);
            if (accessory != null) _augmentation = _augmentation.Augment(accessory.StatsBoost);

            Accessory1 = accessory;

            return this;
        }

        public Agent Accessory2Equip(Accessory? accessory)
        {
            if (Accessory2 != null) _augmentation = _augmentation.Deaugment(Accessory2.StatsBoost);
            if (accessory != null) _augmentation = _augmentation.Augment(accessory.StatsBoost);

            Accessory2 = accessory;

            return this;
        }

        public Agent ActivateArbellum(ArbellumType arbellumType)
        {
            var arbellum = Arbella.First(a => a.Type == arbellumType) ?? throw new ArgumentException($"Agent {Name} has Arbellum of type {arbellumType.Name}");

            arbellum.Activate();

            return this;
        }

        public Agent DeactivateArbellum(ArbellumType arbellumType)
        {
            var arbellum = Arbella.First(a => a.Type == arbellumType) ?? throw new ArgumentException($"Agent {Name} has Arbellum of type {arbellumType.Name}");

            arbellum.Deactivate();

            return this;
        }
    }
}

#nullable disable