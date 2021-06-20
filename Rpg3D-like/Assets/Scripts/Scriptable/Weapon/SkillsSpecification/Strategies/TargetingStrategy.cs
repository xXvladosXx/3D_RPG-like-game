using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable.Weapon
{
    public abstract class TargetingStrategy : ScriptableObject
    {
        public abstract void StartTargeting(SkillData skillData, Action finishedAttack, Action canceledAttack);
    }
}