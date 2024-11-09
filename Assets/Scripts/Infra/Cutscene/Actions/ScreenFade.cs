namespace Cutscene.Actions
{
    public class ScreenFade : Action
    {
        public enum Color
        {
            black,
            white
        };

        public Color To { get; init; }
    }
}