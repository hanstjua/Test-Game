using Battle.Services.Actions;

namespace Battle.Services.Arbella
{
    public class Horology : Arbellum
    {
        public Horology(int experience, bool isActive) : base(
            ArbellumType.Horology, 
            "The forbidden art of daya manipulation to redirect the flow of time.", 
            experience, 
            new Learnable[] {
            },
            isActive
        )
        {}
    }
}