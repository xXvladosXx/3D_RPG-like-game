using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    private int _currentLevel;
    private float _currentExp;

    private FindStat _findStat;
    private Health _health;
    public event Action OnExperienceGained;
    public event Action OnLevelUp;
    
    [SerializeField] private int _level = 1;
    [SerializeField] private ParticleSystem _levelUpEffect;
    private void Awake()
    {
        _findStat = GetComponent<FindStat>();
    }

    private void Start()
    { 
        _level = GetLevel();
        _currentExp = _findStat.GetStat(StatsEnum.Experience);
        
        OnExperienceGained += UpdateLevel;
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
        OnExperienceGained();
    }
    
    private void CalculateNewLevel()
    {
        if (_currentExp > _findStat.GetStat(StatsEnum.ExperienceToLevelUp))
        {
            _currentLevel++;
            print("LevelUpped");
        }
    }
    
    
    void UpdateLevel()
    {
        int newLevel = _findStat.CalculateLevel();

        if (newLevel > GetLevel())
        {
            _level = newLevel;

            OnLevelUp();
            SpawnLevelUpEffect();    
        }
    }

    private void SpawnLevelUpEffect()
    {
        if(_levelUpEffect == null) return;

        ParticleSystem particleSystem = Instantiate(_levelUpEffect, gameObject.transform);
        Destroy(particleSystem, 1f);
    }

    public int GetLevel()
    {
        if (_level < 1)
            _level = _findStat.CalculateLevel();

        return _level;
    }
   
}
