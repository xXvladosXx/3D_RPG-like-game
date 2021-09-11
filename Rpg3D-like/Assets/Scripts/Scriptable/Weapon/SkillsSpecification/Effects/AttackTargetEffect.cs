using System;
using System.Linq;
using Controller;
using Resistance;
using Scriptable.Weapon.SkillsSpecification.Strategies;
using Stats;
using UnityEngine;

namespace Scriptable.Weapon.SkillsSpecification.Effects
{
    [CreateAssetMenu(fileName = "Damaging", menuName = "Abilities/Core/Damage", order = 0)]
    public class AttackTargetEffect : EffectStrategy
    {
        [SerializeField] private float _damage;
        [SerializeField] private float _damageModifier;
        [SerializeField] private DamageType _damageType = DamageType.Physical;
        public override void Effect(SkillData skillData, Action finished)
        {
            float userDamage = 0;

            foreach (var damagePair in skillData.GetUser.GetComponent<Equipment>()
                .GetCurrentWeapon._damage
                .Where(damagePair => damagePair.Key == _damageType)) { userDamage = damagePair.Value; }
        
            foreach (var target in skillData.GetTargets)
            {
                Debug.Log(target);
                if (target.GetComponent<Health>() != null)
                {
                    if (_damage > 0)
                    {
                        target.GetComponent<Health>().RegenerateHealth(_damage + userDamage*_damageModifier);
                    }
                    else
                    {
                        if(!target.GetComponent<Health>().IsDead())
                            target.GetComponent<Health>().TakeDamage(-_damage-(-userDamage*_damageModifier), skillData.GetUser, _damageType);
                    }
                }
            }
        }

        public override void SetData(DataCollector dataCollector)
        {
            dataCollector.AddDataFromNewLine("Damage " + _damage + " * " + _damageModifier);
        }
    }
}
