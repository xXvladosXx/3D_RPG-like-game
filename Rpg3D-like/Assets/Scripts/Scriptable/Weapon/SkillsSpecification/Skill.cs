using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable.Weapon;
using Stats;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Controller
{
    [CreateAssetMenu(fileName = "SkillManager", menuName = "Abilities/Skill", order = 0)]
    public class Skill: ScriptableObject, IAction
    {
        [SerializeField] private float _cooldown;
        [SerializeField] private int _manaCost;
        [SerializeField] private Sprite _image;
        public Sprite GetSkillSprite => _image;

        [SerializeField] private Skill _nextLevelSkill;

        [SerializeField] private Skill[] _previousSkills;
        public Skill GetNextLevelSkill => _nextLevelSkill;
        [SerializeField] private FilterStrategy[] _filterStrategies;
        [SerializeField] private EffectStrategy[] _effectStrategies;
        [SerializeField] private TargetingStrategy _targetingStrategy;

        private GameObject _caster;
        private Animator _animator;
        private CooldownSkillManager _cooldownSkillManager;
        public event Action OnSkillInteract;
        
        public void CasteSkill(GameObject user)
       {
           Mana mana = user.GetComponent<Mana>();

           if (mana.GetCurrentMana() < _manaCost)
           {
               OnSkillInteract?.Invoke();
               return;
           }
 
           if (_animator == null)
           {
               _animator = user.GetComponent<Animator>();
           }

           if(_targetingStrategy == null)
               return;

           _cooldownSkillManager = user.GetComponent<CooldownSkillManager>();
           if (_cooldownSkillManager.GetCooldownSkill(this) > 0)
           {
               return;
           }

           SkillData skillData = new SkillData(user);
           ActionScheduler actionScheduler = user.GetComponent<ActionScheduler>();
            
           actionScheduler.StartAction(skillData);

           _targetingStrategy.StartTargeting(skillData,() =>
           {
               AquireTarget(skillData, mana);
           }, Cancel);
       }

        private void AquireTarget(SkillData skillData, Mana mana)
        { 

            OnSkillInteract?.Invoke();
            
            if(skillData.IsCancelled) return;

            mana.GetComponent<Mana>().CasteSkill(_manaCost);

            _cooldownSkillManager.StartCooldown(this, _cooldown);
            
           foreach(var filterStrategy in _filterStrategies)
           {
               skillData.SetTargets(filterStrategy.Filter(skillData.GetTargets));
           }
           

           foreach (var attackTarget in _effectStrategies)
           {
               skillData.GetUser.transform.LookAt(skillData.GetMousePosition);

               attackTarget.Effect(skillData, EffectFinished);
           }

        }

        private void EffectFinished()
        {
            if (_previousSkills.Length <= 0) return;
            foreach (var skill in _previousSkills)
            {
                skill.CasteSkill(FindObjectOfType<PlayerController>().gameObject);
            }
        }

        public void Cancel()
        {
            OnSkillInteract?.Invoke();
        }
    }
}