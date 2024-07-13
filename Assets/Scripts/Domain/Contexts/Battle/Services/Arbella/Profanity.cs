using Battle.Services.Actions;

namespace Battle.Services.Arbella
{
    public class Profanity : Arbellum
    {
        public Profanity(int experience, bool isActive) : base(
            ArbellumType.Profanity, 
            "Divinity-like technique that manifests and amplifies corruption instead of purity.", 
            experience, 
            new Learnable[] {
            },
            isActive
        )
        {}
    }
}