using Battle.Services.Actions;

namespace Battle.Services.Arbella
{
    public class Viscraft : Arbellum
    {
        public Viscraft(int experience) : base(
            ArbellumType.Viscraft, 
            "The art of chanelling daya into the body to alter its equilibrium state.", 
            experience, 
            new Learnable[] {
            }
        )
        {}
    }
}