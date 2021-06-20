using System.Collections.Generic;

namespace Interface
{
    public interface IModifierStat
    {
        IEnumerable<float> GetStatModifier(StatsEnum stat);
    }
}