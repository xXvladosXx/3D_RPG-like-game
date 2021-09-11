using System;
using UnityEngine;

namespace Scriptable.Weapon.SkillsSpecification.Strategies
{
    public abstract class EffectStrategy : ScriptableObject
    {
        public abstract void Effect(SkillData skillData, Action finished);
        public abstract void SetData(DataCollector dataCollector);
    }
}
