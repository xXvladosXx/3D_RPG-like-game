using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Stats;
using UI.SkillBar;
using UnityEngine;

public class PlayerSkills : MonoBehaviour, IAction
{
    [SerializeField] private Skill[] _playerSkills;

    private GameObject _caster;
    private Animator _animator;
    private SkillBarPlayer _skillBarPlayer;
    private Mana _mana;
    private ActionScheduler _actionScheduler;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _caster = gameObject;
        _skillBarPlayer = GetComponent<SkillBarPlayer>();
        _mana = GetComponent<Mana>();
        _actionScheduler = GetComponent<ActionScheduler>();
    }

    private void Update()
    {
        if(_playerSkills.Length == 0) return;
        
        if (Input.GetKeyDown(KeyCode.Alpha1) && this._playerSkills.Length >= 1)
        {
            CastingSkillOnIndex(_playerSkills[0], 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && this._playerSkills.Length >= 2)
        {
            CastingSkillOnIndex(_playerSkills[1], 1);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3) && this._playerSkills.Length >= 3)
        {
            CastingSkillOnIndex(_playerSkills[2], 2);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4) && this._playerSkills.Length >= 4)
        {
            CastingSkillOnIndex(_playerSkills[3], 3);
        }
    }

    public void CastingSkillOnIndex(Skill skill, int index)
    {
        _actionScheduler.StartAction(this);
        
        skill.CasteSkill(gameObject);
        
        _skillBarPlayer.TriggerToSetFillAmountImage(index,_playerSkills);
    }

    public Skill[] GetPlayerSkills()
    {
        return _playerSkills;
    }
    public void SetPlayerSkills(Skill[] weaponSkills)
    {
        _playerSkills = weaponSkills;
    }

    public void ShowButton(int index)
    {
        if(_playerSkills.Length == 0) return;
        if(index > _playerSkills.Length-1) return;
        
        switch (index )
        {
            case 0:
                CastingSkillOnIndex(_playerSkills[index], index);
                _skillBarPlayer.TriggerToSetFillAmountImage(index, _playerSkills);
                break;
            case 1:
                CastingSkillOnIndex(_playerSkills[index], index);
                _skillBarPlayer.TriggerToSetFillAmountImage(index, _playerSkills);
                break;
            case 2 :
                CastingSkillOnIndex(_playerSkills[index], index);
                _skillBarPlayer.TriggerToSetFillAmountImage(index, _playerSkills);
                break;
            case 3:
                CastingSkillOnIndex(_playerSkills[index], index);
                _skillBarPlayer.TriggerToSetFillAmountImage(index, _playerSkills);
                break;
        }
    }

    public void Cancel()
    {
        
    }
}
