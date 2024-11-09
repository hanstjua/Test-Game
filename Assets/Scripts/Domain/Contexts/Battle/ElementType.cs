namespace Battle
{
    public class ElementType
    {
        public ElementType(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override string ToString()
        {
            return Name;
        }

        public static ElementType Fire = new("Fire");
    }
}