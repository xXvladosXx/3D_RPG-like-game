using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Scriptable.Weapon;
using Stats;
using UI.SkillBar;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    [SerializeField] private Skill[] _playerSkills;
    public Skill[] GetPlayerSkills => _playerSkills;

    public void SetNewLevelSkill(int index)
    {
        _playerSkills[index] = _playerSkills[index].GetNextLevelSkill;
        OnSkillsChanged?.Invoke();
    }
    [SerializeField] private SkillBarPlayer _skillBarPlayer;

    private Skill _previousSkill;
    private Mana _mana;
    private CooldownSkillManager _cooldownSkillManager;
    private bool _isAbleToUseSkill = true;

    public event Action OnSkillsChanged;
    private void Awake()
    {
        _mana = GetComponent<Mana>();
        _cooldownSkillManager = FindObjectOfType<CooldownSkillManager>();
    }

    private void InitPreviousSkill()
    {
        _previousSkill.OnSkillInteract += () => { _isAbleToUseSkill = true; };
    }

    private void Update()
    {
        if(_playerSkills.Length == 0) return;
        
        if (Input.GetKeyDown(KeyCode.Alpha1) && this._playerSkills.Length >= 1 && _isAbleToUseSkill)
        {
            InteractWithSkill(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && this._playerSkills.Length >=2 && _isAbleToUseSkill)
        {
            InteractWithSkill(1);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3) && this._playerSkills.Length >= 3 && _isAbleToUseSkill)
        {
            InteractWithSkill(2);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4) && this._playerSkills.Length >= 4 && _isAbleToUseSkill)
        {
            InteractWithSkill(3);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha5) && this._playerSkills.Length >= 5 && _isAbleToUseSkill)
        {
            InteractWithSkill(4);
        }
        
    }

    private void InteractWithSkill(int index)
    {
        if(_cooldownSkillManager.GetCooldownSkill(_playerSkills[index]) > 0) return;
        
        _isAbleToUseSkill = false;
        CastingSkillOnIndex(_playerSkills[index], index);
        _skillBarPlayer.TriggerCastingSkill(index);
        _previousSkill = _playerSkills[index];
        InitPreviousSkill();
    }

    public void CastingSkillOnIndex(Skill skill, int index)
    {
        skill.CasteSkill(gameObject);
    }

    
    public void SetPlayerSkills(Skill[] weaponSkills)
    {
        _playerSkills = weaponSkills;
        OnSkillsChanged?.Invoke();
    }


   
}
