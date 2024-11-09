namespace Cutscene.Actions
{
    public class Talk : Action
    {
        public enum BubblePosition
        {
            top,
            bottom
        };

        public enum SpeakerAffiliation
        {
            party,
            ally,
            enemy
        };

        public string Speech { get; init; }
        public BubblePosition Position { get; init; }
        public SpeakerAffiliation Affiliation { get; init; }
        public double Duration { get; init; }
        public string Target { get; init; }
    }
}