using System.Collections.Generic;
using Resistance;
using Scriptable.Weapon;
using UnityEngine;

namespace Inventory
{
    public abstract class ArmourObject : ModifiableItem
    {
        [SerializeField] protected ArmourObject _previousChestUpgrade;
        [SerializeField] protected ArmourObject _nextChestUpgrade;
        [SerializeField] protected DamageResistance.Resistance[] _damageResistances;
        [SerializeField] protected DamageType _damageType;
        [SerializeField] protected GameObject _prefab;
        [SerializeField] protected float _armour = 1f;
        [SerializeField] protected float _healthBonus = 1f;
        public float GetHealthBonus => _healthBonus;
        public DamageResistance.Resistance[] GetDamageResistances => _damageResistances;
        public float GetArmourPoints => _armour;

        public void Spawn(Transform position)
        {
            if (_prefab != null)
            {
                GameObject chest = Instantiate(_prefab, position);

                chest.name = Data.Name;
            }
        }
    }
}