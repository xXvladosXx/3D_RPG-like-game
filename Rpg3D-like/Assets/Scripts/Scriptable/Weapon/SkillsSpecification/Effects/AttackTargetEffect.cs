using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable.Weapon;
using UnityEngine;

[CreateAssetMenu(fileName = "DemoTargeting", menuName = "Abilities/AttackTarget", order = 0)]
public class AttackTargetEffect : EffectStrategy
{
    [SerializeField] private float _damage;
    [SerializeField] private float _damageModifier;
    public override void Effect(SkillData skillData, Action finished)
    {
        float userDamage = skillData.GetUser.GetComponent<Combat>().GetCurrentDamage;
        
        foreach (var target in skillData.GetTargets)
        {
            if (target.GetComponent<Health>() != null)
            {
                if (_damage > 0)
                {
                    target.GetComponent<Health>().RegenerateHealth(_damage + userDamage*_damageModifier);
                }
                else
                {
                    if(!target.GetComponent<Health>().IsDead())
                        target.GetComponent<Health>().TakeDamage(-_damage-userDamage*_damageModifier, skillData.GetUser);
                }
            }
        }
    }
}
