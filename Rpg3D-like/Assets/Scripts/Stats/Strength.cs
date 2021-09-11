using System;
using System.Collections.Generic;
using Scriptable.Stats;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public class Strength : StatsModifierContainer
    {
        public SerializableDictionary<StatsEnum, float> _statsModifiers = new SerializableDictionary<StatsEnum, float>();

        public override StatsModifier StatModifier => StatsModifier.Strength;

        public override float GetStatModifier(StatsEnum stat, int points)
        {
            if (_statsModifiers.TryGetValue(stat, out var s))
            {
                return s * points;
            }

            return 0;
        }
    }
}