public class DummyState : IUiState
{
    public IUiState Update(BattleProperties battleProperties)
    {
        return this;
    }
}