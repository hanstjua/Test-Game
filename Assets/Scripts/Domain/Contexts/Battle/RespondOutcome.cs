namespace Battle
{
    public readonly struct RespondOutcome
    {
        public ActionOutcome[] Outcomes { get; }
        public RespondOutcome[] RespondOutcomes { get; }

        public RespondOutcome(ActionOutcome[] outcomes, RespondOutcome[] respondOutcomes)
        {
            Outcomes = outcomes;
            RespondOutcomes = respondOutcomes;
        }
    }
}