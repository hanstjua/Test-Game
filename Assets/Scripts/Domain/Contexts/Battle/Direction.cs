namespace Battle
{
    public enum Direction : int
    {
        North = 1,
        South = -1,
        East = 2,
        West = -2
    }

    public static class Extensions
    {
        public static bool Opposes(this Direction d1, Direction d2)
        {
            return ((int)d1 + (int)d2) == 0;
        }

        public static bool Equals(this Direction d1, Direction d2)
        {
            return (int)d1 == (int)d2;
        }
    }
}