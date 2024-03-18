namespace Battle
{
    public abstract class ActionEffect : ValueObject<string>
    {
        public abstract string Name { get; }
        public AgentId On { get; private set; }
        
        public ActionEffect(AgentId on)
        {
            On = on;
        }

        public abstract void Apply(UnitOfWork unitOfWork);

        public override string ToString()
        {
            return Name;
        }
    }
}