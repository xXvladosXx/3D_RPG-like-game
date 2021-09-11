using System;
using System.Linq;
using Resistance;
using UnityEngine;

namespace Stats
{
    public class Armour : MonoBehaviour
    {
        [SerializeField] private DamageResistance _damageResistance;
        [SerializeField] private string _defaultResistance = "Default Resistance";
        public DamageResistance GetDamageResistance => _damageResistance;

        private void Awake()
        {
            if (_damageResistance == null)
            {
                _damageResistance = Resources.Load<DamageResistance>( _defaultResistance);
            }
            _damageResistance.SetUp();
        }


        public void SetResistance(DamageResistance.Resistance[] resistances, bool b = false)
        {
            foreach (var resistance in resistances)
            {
                if (_damageResistance._resistances.TryGetValue(resistance.DamageType, out var f))
                {
                    _damageResistance._resistances.Remove(resistance.DamageType);
                    _damageResistance._resistances.Add(resistance.DamageType, f + (b ? -resistance.DamageResistance : resistance.DamageResistance));
                }
            }
        }
        
    }
}