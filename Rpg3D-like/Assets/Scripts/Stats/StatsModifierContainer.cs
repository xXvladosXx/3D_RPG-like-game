using System;
using System.Collections.Generic;
using Scriptable.Stats;
using UnityEngine;

namespace Stats
{
    [Serializable]
    public abstract class StatsModifierContainer : MonoBehaviour
    {
        public abstract StatsModifier StatModifier { get; }
        public abstract float GetStatModifier(StatsEnum stat, int points);
    }
}