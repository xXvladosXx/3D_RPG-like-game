using System;
using UnityEngine;

namespace Scriptable.Weapon.SkillsSpecification.Strategies
{
    public abstract class TargetingStrategy : ScriptableObject
    {
        public abstract void StartTargeting(SkillData skillData, Action finishedAttack, Action canceledAttack);
    }
}