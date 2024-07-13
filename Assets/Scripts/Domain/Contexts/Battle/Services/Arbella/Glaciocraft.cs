using Battle.Services.Actions;

namespace Battle.Services.Arbella
{
    public class Glaciocraft : Arbellum
    {
        public Glaciocraft(int experience, bool isActive) : base(
            ArbellumType.Glaciocraft, 
            "Manipulation of daya into frigid elements.", 
            experience, 
            new Learnable[] {
                new(new Ice(), 0, false),
            },
            isActive
        )
        {}
    }
}