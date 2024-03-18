namespace Battle
{
    public readonly struct PreemptOutcome
    {
        public ActionOutcome[] Outcomes { get; }
        public PreemptOutcome[] PreemptOutcomes { get; }

        public PreemptOutcome(ActionOutcome[] outcomes, PreemptOutcome[] preemptOutcomes)
        {
            Outcomes = outcomes;
            PreemptOutcomes = preemptOutcomes;
        }
    }
}