using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBox : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _cooldown;
    [SerializeField] private int _skillIndex;
    
    private PlayerSkills _playerSkills;
    private CooldownSkillManager _cooldownSkillManager;
    private bool _wasCasted;
    private Image _image;
    private int _currentSkillLevel;
    private void Awake()
    {
        _playerSkills = _player.GetComponent<PlayerSkills>();
        _cooldownSkillManager = _player.GetComponent<CooldownSkillManager>();
        _image = _cooldown.GetComponent<Image>();
    }

    private void Update()
    {
        if (_playerSkills.GetPlayerSkills.Length <= _skillIndex)
        {
            _image.fillAmount = 0;
            return;
        }

        if (_wasCasted)
        {
            _image.fillAmount = _cooldownSkillManager.GetFractionOfCooldown(_playerSkills.GetPlayerSkills[_skillIndex]);
        }
    }
    
    public void SetCasted(bool wasCasted)
    {
        _wasCasted = wasCasted;
    }
}
