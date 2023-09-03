using Battle;


public abstract class ActionHandler
{
    public ActionHandler(Action actionService)
    {
        Service = actionService;
    }

    public Action Service { get; private set; }

    public abstract IUiState Handle(BattleProperties battleProperties, IUiState onCancelState, IUiState onProceedState);
    public abstract bool ValidateTarget(Agent agent);
    public abstract IUiState ExecuteAction(Agent target);
    public abstract IUiState CancelAction();
}