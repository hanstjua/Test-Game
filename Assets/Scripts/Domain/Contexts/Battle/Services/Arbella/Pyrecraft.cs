using Battle.Services.Actions;

namespace Battle.Services.Arbella
{
    public class Pyrecraft : Arbellum
    {
        public Pyrecraft(int experience, bool isActive) : base(
            ArbellumType.Pyrecraft, 
            "Manipulation of daya into explosive forces.", 
            experience, 
            new Learnable[] {
                new(new Fire(), 0, false),
            },
            isActive
        )
        {}
    }
}