using System.Linq;
using Battle;


class ActionOutcomesHandler : IUiState
{
    private readonly struct OutcomeNode
    {
        public ActionOutcome Outcome { get; }
        public ActionOutcome Parent { get; }
        public ActionOutcome[] Children { get; }

        public  OutcomeNode(ActionOutcome outcome, ActionOutcome parent, ActionOutcome[] children)
        {
            Outcome = outcome;
            Parent = parent;
            Children = children;
        }
    }
    private readonly struct Node
    {
        public object Content { get; }
        public object Parent { get; }
        public object[] Children { get; }
        public Node(object content, object parent, object[] children)
        {
            Content = content;
            Parent = parent;
            Children = children;
        }
    }
    private interface INodeState
    {
        public IUiState NextState();
    }

    private class PreemptOutcomesHandler : IUiState
    {
        private PreemptOutcome _outcome;
        private IUiState _nextState;
        private bool _hasInit = false;

        public PreemptOutcomesHandler(PreemptOutcome outcome, IUiState nextState)
        {
            _outcome = outcome;
            _nextState = nextState;
        }

        public IUiState Update(BattleProperties battleProperties)
        {
            if (!_hasInit)
            {
                // Animate preempt banner

                _hasInit = true;
            }

            if (false)  // if preempt banner still animating
            {
                return this;
            }
            else
            {
                if (_outcome.PreemptOutcomes.Count() > 0)
                {
                    return _outcome
                    .PreemptOutcomes
                    .Reverse()
                    .Aggregate(
                        new PreemptOutcomesHandler(
                            _outcome, 
                            ExecutionStateDispatcher.Dispatch(_outcome.Outcomes, _nextState)
                        ), 
                        (state, outcome) => new PreemptOutcomesHandler(outcome, state)
                    );
                }
                else
                {
                    return ExecutionStateDispatcher.Dispatch(_outcome.Outcomes, _nextState);
                }
            }
        }
    }

    private class ActionOutcomeHandler : IUiState
    {
        private ActionOutcome _outcome;
        private RespondOutcomesHandler _nextHandler;
        public ActionOutcomeHandler(ActionOutcome outcome, RespondOutcomesHandler nextHandler)
        {
            _outcome = outcome;
            _nextHandler = nextHandler;
        }

        public IUiState Update(BattleProperties battleProperties)
        {
            throw new System.NotImplementedException();
        }
    }

    private class RespondOutcomesHandler : IUiState
    {
        private RespondOutcome _outcome;
        private IUiState _nextState;
        private bool _hasInit = false;
        public RespondOutcomesHandler(RespondOutcome outcome, IUiState nextState)
        {
            _outcome = outcome;
            _nextState = nextState;
        }

        public IUiState Update(BattleProperties battleProperties)
        {
            if (!_hasInit)
            {
                // Animate preempt banner

                _hasInit = true;
            }

            if (false)  // if preempt banner still animating
            {
                return this;
            }
            else
            {
                if (_outcome.RespondOutcomes.Count() > 0)
                {
                    return this;
                }
                else
                {
                    return ExecutionStateDispatcher.Dispatch(_outcome.Outcomes, _nextState);
                }
            }
        }
    }

    private PreemptOutcome[] _preemptOutcomes;
    private ActionOutcome _actionOutcome;
    private RespondOutcome[] _respondOutcomes;
    private IUiState _nextState;

    public ActionOutcomesHandler(PreemptOutcome[] preemptOutcomes, ActionOutcome actionOutcome, RespondOutcome[] respondOutcomes, IUiState nextState)
    {
        _preemptOutcomes = preemptOutcomes;
        _actionOutcome = actionOutcome;
        _respondOutcomes = respondOutcomes;
        _nextState = nextState;
    }

    public IUiState Update(BattleProperties battleProperties)
    {
        return this;
    }
}