﻿using System;
using UnityEngine;

namespace Scriptable.Weapon
{
    [CreateAssetMenu(fileName = "AnimationTriggering", menuName = "Abilities/SpellCastingAnimation", order = 0)]
    
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
    }
}