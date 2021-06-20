using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable.Weapon;
using UnityEngine;

public abstract class EffectStrategy : ScriptableObject
{
    public abstract void Effect(SkillData skillData, Action finished);
}
