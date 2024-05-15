using Battle.Services.Actions;

namespace Battle.Services.Arbella
{
    public class Supplies : Arbellum
    {
        public Supplies(int experience) : base(
            ArbellumType.Supplies, 
            "Use battle supplies.", 
            experience, 
            new Learnable[] {
            }
        )
        {}
    }
}