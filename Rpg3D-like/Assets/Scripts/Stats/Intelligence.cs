using Scriptable.Stats;

namespace Stats
{
    public class Intelligence : StatsModifierContainer
    {
        public override StatsModifier StatModifier { get; }
        public override float GetStatModifier(StatsEnum stat, int points)
        {
            throw new System.NotImplementedException();
        }
    }
}