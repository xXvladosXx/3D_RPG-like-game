using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    private int _currentLevel;
    private float _currentExp;
    private FindStat _findStat;
    public event Action OnExperienceGained;
    
    [SerializeField] private ParticleSystem _levelUpEffect;
    private void Awake()
    {
        _findStat = GetComponent<FindStat>();
    }

    private void Start()
    { 
        _currentExp = _findStat.GetStat(StatsEnum.Experience);
    }

    public float GetExperience()
    {
        return _currentExp;
    }

    public float GetExperienceToLevelUp()
    {
        return _findStat.GetStat(StatsEnum.ExperienceToLevelUp);
    }
    
    public void CalculateExperience(float experience)
    {
        _currentExp += experience;
        if (OnExperienceGained != null) OnExperienceGained();
    }
    
  
    public void SpawnLevelUpEffect()
    {
        if(_levelUpEffect == null) return;

        ParticleSystem particleSystem = Instantiate(_levelUpEffect, gameObject.transform);
        Destroy(particleSystem, 1f);
    }
}
