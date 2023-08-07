using System;

namespace Battle
{
    [Serializable]
    public class BattleId : ValueObject<string>
    {
        private string _uuid;

        public BattleId(string uuid)
        {
            _uuid = uuid;
        }

        public override string Value()
        {
            return _uuid;
        }
    }
}