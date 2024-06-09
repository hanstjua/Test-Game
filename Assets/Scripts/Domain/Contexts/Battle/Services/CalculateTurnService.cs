using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
    public class CalculateTurnService
    {
        public List<Agent> Execute(List<Agent> agents)
        {
            // Turn gauges map
            var gauges = new Dictionary<Agent, double>();

            foreach (var agent in agents)
            {
                gauges.Add(agent, agent.TurnGauge);
            }

            // Calculate the next N turns
            int N = 30;
            double turnThreshold = 100.0;
            List<Agent> turns = new();

            while (turns.Count < N)
            {
                // increment everyone's gauge
                foreach (var item in gauges)
                {
                    gauges[item.Key] = item.Value + item.Key.TurnGaugeIncrement;
                }

                // filter agents that hits the threshold
                var passedThreshold = new List<Agent>();
                foreach (var item in gauges)
                {
                    if (item.Value >= turnThreshold)
                    {
                        passedThreshold.Add(item.Key);
                        gauges[item.Key] -= turnThreshold;
                    }
                }

                // update turns list
                if (passedThreshold.Count > 0)
                {
                    turns = turns.Concat(passedThreshold.OrderBy(agent => gauges[agent])).ToList();
                }
            }

            return turns;
        }
    }
}