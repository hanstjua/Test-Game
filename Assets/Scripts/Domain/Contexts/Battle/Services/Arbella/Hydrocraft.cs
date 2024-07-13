using Battle.Services.Actions;

namespace Battle.Services.Arbella
{
    public class Hydrocraft : Arbellum
    {
        public Hydrocraft(int experience, bool isActive) : base(
            ArbellumType.Hydrocraft, 
            "Manipulation of daya into torrential flows.", 
            experience, 
            new Learnable[] {
                new(new Water(), 0, false),
            },
            isActive
        )
        {}
    }
}