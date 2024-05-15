using Battle.Services.Actions;

namespace Battle.Services.Arbella
{
    public class Malediction : Arbellum
    {
        public Malediction(int experience) : base(
            ArbellumType.Malediction, 
            "The mysterious ancient art of daya manipulation devised by Nymore Nausk, the Calamity Embodied.", 
            experience, 
            new Learnable[] {
            }
        )
        {}
    }
}