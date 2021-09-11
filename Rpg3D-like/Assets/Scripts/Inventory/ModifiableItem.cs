using Scriptable.Stats;
using UnityEngine;

namespace Inventory
{
    public abstract class ModifiableItem : ItemObject
    {
        [SerializeField] protected SerializableDictionary<StatsEnum, float> modifiers = new SerializableDictionary<StatsEnum, float>();
        public float PriceToUpgrade;
        
        public float GetModifiers(StatsEnum stat)
        {
            if (!modifiers.TryGetValue(stat, out var m))
            {
                return 0;
            }

            return m;
        }

    }
}