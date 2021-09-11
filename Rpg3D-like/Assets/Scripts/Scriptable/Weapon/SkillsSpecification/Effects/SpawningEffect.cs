using System;
using Scriptable.Weapon.SkillsSpecification.Strategies;
using UnityEngine;

namespace Scriptable.Weapon.SkillsSpecification.Effects
{
    [CreateAssetMenu(fileName = "DelayClickTargeting", menuName = "Abilities/Core/Effect", order = 0)]
    public class SpawningEffect : EffectStrategy
    {
        [SerializeField] private GameObject _spell;

        public override void Effect(SkillData skillData, Action finished)
        {
            SpawnEffect(skillData, finished);
        }

        public override void SetData(DataCollector dataCollector)
        {
            
        }

        private void SpawnEffect(SkillData skillData, Action finished)
        {
            GameObject spell = Instantiate(_spell, skillData.GetMousePosition, Quaternion.identity);
            spell.transform.position = new Vector3(skillData.GetMousePosition.x, skillData.GetMousePosition.y, skillData.GetMousePosition.z);
            
            Destroy(spell, 3f);
            finished();
        }
    }
}