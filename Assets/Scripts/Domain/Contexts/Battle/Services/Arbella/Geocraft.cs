using Battle.Services.Actions;

namespace Battle.Services.Arbella
{
    public class Geocraft : Arbellum
    {
        public Geocraft(int experience) : base(
            ArbellumType.Geocraft, 
            "Manipulation of daya into earth deformation.", 
            experience, 
            new Learnable[] {
                new(new Earth(), 0, false),
            }
        )
        {}
    }
}