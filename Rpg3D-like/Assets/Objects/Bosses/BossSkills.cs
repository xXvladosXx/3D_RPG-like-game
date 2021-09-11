using System;
using BehaviorDesigner.Runtime;
using Controller;
using Scriptable.Weapon.SkillsSpecification;
using Stats;
using UnityEngine;

namespace DefaultNamespace.Objects.Bosses
{
    [RequireComponent(typeof(Mana))]
    public class BossSkills : MonoBehaviour
    {
        [SerializeField] private Skill[] _skills;
        public SharedFloat _SharedFloat;

        public Skill GetSkillOnIndex(int index)
        {
            return _skills[index];
        }

        public void CastSkill(int index)
        {
            if(_skills == null) return;
            if(index >= _skills.Length) return;

            _skills[index].CastSkill(gameObject);
            _SharedFloat.Value = 0;
        }
    }
}