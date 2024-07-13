using Battle.Services.Actions;

namespace Battle.Services.Arbella
{
    public class Divinity : Arbellum
    {
        public Divinity(int experience, bool isActive) : base(
            ArbellumType.Divinity, 
            "The holy art of harnessing daya to manifest divine purifying energy.", 
            experience, 
            new Learnable[] {
            },
            isActive
        )
        {}
    }
}