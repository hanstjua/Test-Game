namespace Battle
{
    public class SkillType
    {
        public SkillType(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public static readonly SkillType Physical = new("Physical");
        public static readonly SkillType Item = new("Item");
    }
}