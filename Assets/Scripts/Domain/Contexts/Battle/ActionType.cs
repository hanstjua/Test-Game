using PlasticPipe.PlasticProtocol.Messages;

namespace Battle
{
    public class ActionType
    {
        public ActionType(string name)
        {
            Name = name;   
        }

        public string Name { get; private set; }

        public override string ToString()
        {
            return Name;
        }

        // special types
        public static readonly ActionType PreemptTriggered = new("PreemptTriggered");
        public static readonly ActionType PreemptAnnulled = new("PreemptAnnulled");
        public static readonly ActionType RespondTriggered = new("RespondTriggered");

        // Physical
        public static readonly ActionType Attack = new("Attack");
        public static readonly ActionType Defend = new("Defend");

        // Supplies
        public static readonly ActionType UseItem = new("UseItem");

        // Pyrecraft
        public static readonly ActionType Fire = new("Fire");
        public static readonly ActionType Explosion = new("Explosion");
        public static readonly ActionType Embers = new("Embers");
        public static readonly ActionType WillOTheWisp = new("Will-o'-the-wisp");

        // Hydrocraft
        public static readonly ActionType Water = new("Water");
        public static readonly ActionType Blast = new("Blast");
        public static readonly ActionType Drench = new("Drench");

        // Glaciocraft
        public static readonly ActionType Ice = new("Ice");

        // Brontocraft
        public static readonly ActionType Thunder = new("Thunder");

        // Aercraft
        public static readonly ActionType Wind = new("Wind");

        // Geocraft
        public static readonly ActionType Earth = new("Earth");

        // Divinity
        public static readonly ActionType Holy = new("Holy");

        // Profanity
        public static readonly ActionType Dark = new("Dark");

        // Physics
        public static readonly ActionType Pull = new("Pull");
        public static readonly ActionType Push = new("Push");
        public static readonly ActionType Teleport = new("Teleport");
        public static readonly ActionType SpatialCompression = new("Spatial Compression");
        public static readonly ActionType VeilOfInfinity = new("Veil of Infinity");

        // Horology
        public static readonly ActionType Haste = new("Haste");
        public static readonly ActionType Slow = new("Slow");
        public static readonly ActionType Rewind = new("Rewind");
        public static readonly ActionType Conclude = new("Conclude");

        // Viscraft
        public static readonly ActionType Empower = new("Empower");
        public static readonly ActionType Vitalise = new("Vitalise");
        public static readonly ActionType Recover = new("Recover");
        public static readonly ActionType Rejuvenate = new("Rejuvenate");
        public static readonly ActionType Revive = new("Revive");
        public static readonly ActionType SecondWind = new("Second Wind");

        // Malediction
        public static readonly ActionType Disintegrate = new("Disintegrate");
        public static readonly ActionType Sever = new("Sever");
        public static readonly ActionType AltarOfDamnation = new("Altar of Damnation");
    }
}