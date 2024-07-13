using Battle.Services.Actions;

namespace Battle.Services.Arbella
{
    public class Supplies : Arbellum
    {
        public Supplies(int experience, bool isActive) : base(
            ArbellumType.Supplies, 
            "Use battle supplies.", 
            experience, 
            new Learnable[] {
            },
            isActive
        )
        {}
    }
}