using System;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public class LevelUpService
    {
        public static void Execute(ActionOutcome outcome, Action action, UnitOfWork unitOfWork)
        {
            var isActionSuccessful = outcome.Effects.Aggregate(true, (value, effect) => value && effect is ActionFailed);
            if (isActionSuccessful)
            {
                using (unitOfWork)
                {
                    var actor = unitOfWork.AgentRepository.Get(outcome.By);
                    var targets = outcome.On.Select(i => unitOfWork.AgentRepository.Get(i));

                    var increments = targets.Append(actor).ToDictionary(a => a, _ => new Dictionary<StatType, uint>());

                    // Actor stat increments
                    foreach (var stat in action.ActorRelevantStats)
                    {
                        uint inc = 0;

                        if (stat == StatType.Strength)
                        {
                            var meanDefense = targets.Aggregate(0, (d, a) => d + a.Stats.Defense) / targets.Count();
                            inc = (uint) AntagonisticStatsLevelsActivation(actor.Stats.Strength - meanDefense);
                        }
                        else if (stat == StatType.Magic)
                        {
                            var meanMagicDefense = targets.Aggregate(0, (d, a) => d + a.Stats.MagicDefense) / targets.Count();
                            inc = (uint) AntagonisticStatsLevelsActivation(actor.Stats.Magic - meanMagicDefense);
                        }

                        increments[actor].Add(stat, inc);
                    }

                    // Targets stat increments
                    foreach (var stat in action.TargetRelevantStats)
                    {
                        uint inc = 0;
                        
                        if (stat == StatType.Defense)
                        {
                            var meanDefense = targets.Aggregate(0, (d, a) => d + a.Stats.Defense) / targets.Count();
                            inc = (uint) AntagonisticStatsLevelsActivation(meanDefense - actor.Stats.Strength);
                        }
                        else if (stat == StatType.MagicDefense)
                        {
                            var meanMagicDefense = targets.Aggregate(0, (d, a) => d + a.Stats.MagicDefense) / targets.Count();
                            inc = (uint) AntagonisticStatsLevelsActivation(meanMagicDefense - actor.Stats.Magic);
                        }

                        foreach (var kvp in increments.Where(kvp => !kvp.Value.Equals(actor)))
                        {
                            kvp.Value.Add(stat, inc);
                        }
                    }

                    foreach (var effect in outcome.Effects)
                    {
                        if (effect is HpDamage hpDamage)
                        {
                            var agent = unitOfWork.AgentRepository.Get(effect.On);
                            var inc = (uint) ExhaustionBasedLevelsActivation(hpDamage.Amount, agent.Stats.MaxHp);
                            increments[agent].Add(StatType.MaxHp, inc);
                        }
                        else if (effect is MpDamage mpDamage)
                        {
                            var agent = unitOfWork.AgentRepository.Get(effect.On);
                            var inc = (uint) ExhaustionBasedLevelsActivation(mpDamage.Amount, agent.Stats.MaxMp);
                            increments[agent].Add(StatType.MaxMp, inc);
                        }
                    }

                    // TODO: level up agility, evasion, luck

                    var updatedAgents = increments.Select(inc => inc.Key.LevelsUp(inc.Value));
                    foreach (var agent in updatedAgents)
                    {
                        unitOfWork.AgentRepository.Update(agent.Id() as AgentId, agent);
                    }

                    // Actor arbellum level up
                    unitOfWork.AgentRepository.Update(actor.Id() as AgentId, actor.ArbellumUp(action.Arbellum, 1));

                    unitOfWork.Save();
                }
            }
        }

        // for levels up calc: maps (str - def) to str lvl and def lvl
        public static double AntagonisticStatsLevelsActivation(double statsDelta)
        {
            return (StatLevels.MAX_LEVEL + 1) / 100 / (1 + Math.Pow(Math.E, statsDelta / 100));
        }

        // for levels up calc: maps damage to hp and mp lvl
        public static double ExhaustionBasedLevelsActivation(double damage, int max)
        {
            return damage + 2 / (1 + Math.Pow(Math.E, max / 1889)) * 0.05;
        }
    }
}
