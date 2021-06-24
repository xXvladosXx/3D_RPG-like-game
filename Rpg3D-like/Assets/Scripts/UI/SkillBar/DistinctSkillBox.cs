using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistinctSkillBox : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private PlayerSkills _playerSkills;
    private CooldownSkillManager _cooldownSkillManager;
    private Image _image;
    private bool _wasCasted;
    private void Awake()
    {
        _playerSkills = _player.GetComponent<PlayerSkills>();
        _cooldownSkillManager = _player.GetComponent<CooldownSkillManager>();
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        if (_wasCasted)
        {
            _cooldownSkillManager.GetFractionOfCooldown(_playerSkills.GetPlayerSkills()[0]);
        }
    }
}
