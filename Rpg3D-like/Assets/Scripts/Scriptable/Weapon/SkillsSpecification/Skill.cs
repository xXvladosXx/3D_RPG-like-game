using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable.Weapon;
using Stats;
using UnityEngine;
using UnityEngine.AI;

namespace Controller
{
    [CreateAssetMenu(fileName = "SkillManager", menuName = "Abilities/Skill", order = 0)]
    public class Skill: ScriptableObject, IAction
    {
        [SerializeField] private float _cooldown;
        [SerializeField] private int _manaCost;
        [SerializeField] private FilterStrategy[] _filterStrategies;
        [SerializeField] private EffectStrategy[] _effectStrategies;
        [SerializeField] private TargetingStrategy _targetingStrategy;

        private GameObject _caster;
        private Animator _animator;
        private CooldownSkillManager _cooldownSkillManager;
        
        public void CasteSkill(GameObject user)
       {
           Mana mana = user.GetComponent<Mana>();
           
           if(mana.GetCurrentMana() < _manaCost) return;
 
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
            
            TriggerSkill("isCasting", true);

            SkillData skillData = new SkillData(user);
            ActionScheduler actionScheduler = user.GetComponent<ActionScheduler>();
            
            actionScheduler.StartAction(skillData);
            
            _targetingStrategy.StartTargeting(skillData,() =>
            {
                AquireTarget(skillData, mana);
            }, CancelTargeting);
       }

        private void AquireTarget(SkillData skillData, Mana mana)
        {
            if(skillData.IsCancelled) return;
            
            WaitUntilSkillCasted();
            
            mana.GetComponent<Mana>().CasteSkill(_manaCost);

            _cooldownSkillManager.StartCooldown(this, _cooldown);
            
           foreach(var filterStrategy in _filterStrategies)
           {
               skillData.SetTargets(filterStrategy.Filter(skillData.GetTargets));
           }
           
           skillData.GetUser.transform.LookAt(skillData.GetMousePosition);

           foreach (var attackTarget in _effectStrategies)
           {
               attackTarget.Effect(skillData, EffectFinished);
           }

           TriggerSkill("isCasting", false);
        }

        private void EffectFinished()
        {
            
        }

        private IEnumerator WaitUntilSkillCasted()
        {
            yield return null;
        }

        private void CancelTargeting()
        {
            TriggerSkill("isCasting", false);
        }

        private void TriggerSkill(String animation, bool flag)
        {
            _animator.SetBool(animation, flag);
        }

        public void Cancel()
        {
            
        }
    }
}