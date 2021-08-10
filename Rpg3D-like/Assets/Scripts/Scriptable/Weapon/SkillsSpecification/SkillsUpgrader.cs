using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controller;
using UnityEngine;
using UnityEngine.UI;

public class SkillsUpgrader : MonoBehaviour
{
    private Button _skillUpgrade;
    private PlayerSkills _playerSkills;
    private int _indexToUpgrade;
    
    public event Action OnSkillDistributed;
    private void Awake()
    {
        _playerSkills = FindObjectOfType<PlayerSkills>();
        _skillUpgrade = GetComponent<Button>();
        _indexToUpgrade = GetComponentInParent<SkillBox>().GetIndex;
        _skillUpgrade.onClick.AddListener(() => UpgradeSkill());
    }

    private void Update()
    {
        if (_indexToUpgrade >= _playerSkills.GetPlayerSkills.Length)
        {
            GetComponent<Button>().interactable = false;
            return;
        }
        
        if (_playerSkills.GetPlayerSkills.Length == 0 || _playerSkills.GetPlayerSkills[_indexToUpgrade].GetNextLevelSkill == null)
        {
            GetComponent<Button>().interactable = false;
        }
    }

    private void UpgradeSkill()
    {
        if (_indexToUpgrade < _playerSkills.GetPlayerSkills.Length && _playerSkills.GetPlayerSkills[_indexToUpgrade].GetNextLevelSkill != null)
        {
            OnSkillDistributed?.Invoke();
            _playerSkills.SetNewLevelSkill(_indexToUpgrade);
        }
    }
}
