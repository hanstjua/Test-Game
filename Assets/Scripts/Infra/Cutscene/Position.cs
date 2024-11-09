namespace Cutscene
{
    public class Position
    {
        public const string ABSOLUTE = "absolute";
        public const string RELATIVE = "relative";
        
        public string Mode { get; init; }
        public int[] Coord { get; init; }
    }
}