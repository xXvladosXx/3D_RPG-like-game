using System;
using System.Collections;
using Scriptable.Weapon.SkillsSpecification.Strategies;
using UnityEngine;

namespace Scriptable.Weapon.SkillsSpecification.Effects
{
    [CreateAssetMenu(fileName = "Delay Damaging", menuName = "Abilities/Optional/SkillDelay", order = 0)]
    public class DelayDamageEffect : EffectStrategy
    {
        [SerializeField] private float _delayToDamage = 0;
        [SerializeField] private EffectStrategy[] _delayedEffects;
        public override void Effect(SkillData skillData, Action finished)
        {
            skillData.StartCoroutine(DelayedEffects(skillData, finished));
        }

        public override void SetData(DataCollector dataCollector)
        {
            dataCollector.AddDataFromNewLine("Delay " + _delayToDamage);
        }

        private IEnumerator DelayedEffects(SkillData skillData, Action finished)
        {
            yield return new WaitForSeconds(_delayToDamage);
            foreach (var effect in _delayedEffects)
            {
                effect.Effect(skillData, finished);
            }
            
            if(skillData.GetRenderer() != null)
                skillData.GetRenderer().gameObject.SetActive(false);
        }
    }
}