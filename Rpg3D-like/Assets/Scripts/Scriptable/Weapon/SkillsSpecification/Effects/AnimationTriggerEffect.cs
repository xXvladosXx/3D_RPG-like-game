using System;
using Scriptable.Weapon.SkillsSpecification.Strategies;
using UnityEngine;

namespace Scriptable.Weapon.SkillsSpecification.Effects
{
    [CreateAssetMenu(fileName = "AnimationTriggering", menuName = "Abilities/Optional/SkillAnimation", order = 0)]
    
    public class AnimationTriggerEffect : EffectStrategy
    {
        [SerializeField] private AnimatorOverrideController _animator;
        [SerializeField] private string _animationSkill = "";
        private Animator _userAnimator;
        public override void Effect(SkillData skillData, Action finished)
        {
            _userAnimator = skillData.GetUser.GetComponent<Animator>();

            if(_animator!=null)
                _userAnimator.runtimeAnimatorController = _animator;

            _userAnimator.SetTrigger(_animationSkill);

            finished();
        }

        public override void SetData(DataCollector dataCollector)
        {
        }
    }
}