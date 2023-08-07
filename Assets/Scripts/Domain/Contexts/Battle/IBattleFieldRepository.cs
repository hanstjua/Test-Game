namespace Battle
{
    public interface IBattleFieldRepository
    {
        public BattleField Get(BattleFieldId id);
        public BattleField GetBattleFieldByName(string name);
        public BattleFieldId CreateBattleField(Terrain[][] terrains);
        public IBattleFieldRepository Reload();
        public IBattleFieldRepository Save();
    }
}