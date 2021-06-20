using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;

public class CooldownSkillManager : MonoBehaviour
{
    private Dictionary<Skill, float> _cooldownTimers = new Dictionary<Skill, float>();
    private Dictionary<Skill, float> _currentCooldownTimer = new Dictionary<Skill, float>();
    private void Update()
    {
        var key = new List<Skill>(_cooldownTimers.Keys);

        foreach (var skillCooldownTimer in key)
        {
            _cooldownTimers[skillCooldownTimer] -= Time.deltaTime;

            if (_cooldownTimers[skillCooldownTimer] < 0)
            {
                _cooldownTimers.Remove(skillCooldownTimer);
                _currentCooldownTimer.Remove(skillCooldownTimer);
            }
        }
    }

    public void StartCooldown(Skill skill, float cooldown)
    {
        _cooldownTimers[skill] = cooldown;
        _currentCooldownTimer[skill] = cooldown;
    }

    public float GetCooldownSkill(Skill skill)
    {
        if (!_cooldownTimers.ContainsKey(skill))
        {
            return 0;
        }

        return _cooldownTimers[skill];
    }

    public float GetFractionOfCooldown(Skill skill)
    {
        if (skill == null)
        {
            return 0;
        }
        
        if (!_cooldownTimers.ContainsKey(skill))
        {
            return 0;
        }

        return _cooldownTimers[skill] / _currentCooldownTimer[skill]; 
    }
}
