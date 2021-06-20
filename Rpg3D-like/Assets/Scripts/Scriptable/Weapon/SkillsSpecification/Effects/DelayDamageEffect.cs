using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable.Weapon
{
    [CreateAssetMenu(fileName = "Delay Damaging", menuName = "Abilities/SkillDelay", order = 0)]
    public class DelayDamageEffect : EffectStrategy
    {
        [SerializeField] private float _delayToDamage = 0;
        [SerializeField] private EffectStrategy[] _delayedEffects;
        public override void Effect(SkillData skillData, Action finished)
        {
            skillData.StartCoroutine(DelayedEffects(skillData, finished));
        }

        private IEnumerator DelayedEffects(SkillData skillData, Action finished)
        {
            yield return new WaitForSeconds(_delayToDamage);

            foreach (var effect in _delayedEffects)
            {
                effect.Effect(skillData, finished);
            }
        }
    }
}