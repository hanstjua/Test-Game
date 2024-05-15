using Battle.Services.Actions;

namespace Battle.Services.Arbella
{
    public class Physical : Arbellum
    {
        public Physical(int experience) : base(
            ArbellumType.Physical, 
            "Basic combat skills.", 
            experience, 
            new Learnable[] {
                new(new Attack(), 0, false),
                new(new Defend(), 0, false),
                new(new UseItem(), 0, false)
            }
        )
        {}
    }
}