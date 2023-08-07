using System;
using System.Collections.Generic;

namespace Battle
{
    public class ActionOutcome : ValueObject<(string, int?, int?, Status[], Status[])>
    {
        public enum FailureType
        {
            Missed,
            Parry,
            Block,
            Immune
        }

        public ActionOutcome(
            AgentId on,
            int hpDamage = 0,
            int mpDamage = 0,
            Status[] addStatuses = null,
            Status[] removeStatuses = null,
            FailureType? failure = null
        )
        {
            On = on;
            HpDamage = hpDamage;
            MpDamage = mpDamage;
            AddStatuses = addStatuses;
            RemoveStatuses = removeStatuses;
        }

        public AgentId On { get; private set; }
        public int HpDamage { get; private set; }
        public int MpDamage { get; private set; }
        public Status[] AddStatuses { get; private set; }
        public Status[] RemoveStatuses { get; private set; }
        public FailureType? Failure { get; private set; }

        public override (string, int?, int?, Status[], Status[]) Value()
        {
            return (On.Value(), HpDamage, MpDamage, AddStatuses, RemoveStatuses);
        }
    }
}