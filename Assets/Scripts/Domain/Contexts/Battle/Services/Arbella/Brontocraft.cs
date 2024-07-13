using Battle.Services.Actions;

namespace Battle.Services.Arbella
{
    public class Brontocraft : Arbellum
    {
        public Brontocraft(int experience, bool isActive) : base(
            ArbellumType.Brontocraft, 
            "Manipulation of daya into destructive flashes.", 
            experience, 
            new Learnable[] {
                new(new Thunder(), 0, false)
            },
            isActive
        )
        {}
    }
}