using Battle.Services.Actions;

namespace Battle.Services.Arbella
{
    public class Aercraft : Arbellum
    {
        public Aercraft(int experience, bool isActive) : base(
            ArbellumType.Aercraft, 
            "Manipulation of daya into turbulent gusts.", 
            experience, 
            new Learnable[] {
                new(new Wind(), 0, false)
            },
            isActive
        )
        {}
    }
}