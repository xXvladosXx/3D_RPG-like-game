using System;
using System.Collections;
using System.Collections.Generic;
using UI.SkillBar;
using UnityEngine;
using UnityEngine.UI;

public class SkillPointsDistributor : MonoBehaviour
{
    [SerializeField] private int _pointsToUpgradeSkill = 1;

    private SkillBarPlayer _skillBarPlayer;
    private List<SkillsUpgrader> _skillsUpgraders;
    private bool _canDistribute = true;
    private FindStat _playerLevel;
    private void Awake()
    {
        _skillBarPlayer = FindObjectOfType<SkillBarPlayer>();
        _playerLevel = FindObjectOfType<PlayerController>().GetComponent<FindStat>();

        _skillsUpgraders = new List<SkillsUpgrader>();
        foreach (var skillsUpgrader in FindObjectsOfType<SkillsUpgrader>())
        {
            _skillsUpgraders.Add(skillsUpgrader);
        }
        _playerLevel.OnLevelUp += () => _pointsToUpgradeSkill++;

        if (_pointsToUpgradeSkill <= 0) return;
        
        foreach (var skill in _skillsUpgraders)
        {
            skill.OnSkillDistributed += () => _pointsToUpgradeSkill--;
        }
    }

    private void Update()
    {
        AvailableToDistribute();
    }

    private void AvailableToDistribute()
    {
        foreach (var skill in _skillsUpgraders)
        {
            skill.GetComponent<Button>().interactable = _pointsToUpgradeSkill > 0;
        }
    }
}
