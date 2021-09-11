using System.Collections.Generic;
using Scriptable.Stats;

namespace Stats
{
    public interface IModifierStat
    {
        IEnumerable<float> GetStatModifier(StatsEnum stat);
    }
}