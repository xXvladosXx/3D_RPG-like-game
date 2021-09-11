using System;
using System.Collections;
using Controller;
using Scriptable.Weapon.SkillsSpecification.Strategies;
using UnityEngine;

namespace Scriptable.Weapon.SkillsSpecification.Effects
{
    public abstract class TeleportEffect : EffectStrategy
    {
        [SerializeField] protected GameObject _spawnEffect;

        protected abstract void Teleporting(SkillData skillData, Vector3 position);

    }
}