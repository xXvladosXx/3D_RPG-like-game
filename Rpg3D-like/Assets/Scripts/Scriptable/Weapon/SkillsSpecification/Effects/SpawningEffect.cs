using System;
using UnityEngine;

namespace Scriptable.Weapon
{
    [CreateAssetMenu(fileName = "DelayClickTargeting", menuName = "Abilities/SpawnEffect", order = 0)]
    public class SpawningEffect : EffectStrategy
    {
        [SerializeField] private GameObject _spell;

        public override void Effect(SkillData skillData, Action finished)
        {
            SpawnEffect(skillData, finished);
        }

        private void SpawnEffect(SkillData skillData, Action finished)
        {
            GameObject spell = Instantiate(_spell, skillData.GetMousePosition, Quaternion.identity);
            spell.transform.position = new Vector3(skillData.GetMousePosition.x, skillData.GetMousePosition.y + 2, skillData.GetMousePosition.z);
            finished();
        }
    }
}