using System;
using System.Collections;
using System.Collections.Generic;
using Saving;
using UnityEngine;

public class LevelUp : MonoBehaviour, ISaveable
{
    [SerializeField] private float _currentExp;
    private FindStat _findStat;
    public event Action OnExperienceGained;
    
    [SerializeField] private ParticleSystem _levelUpEffect;
    private void Awake()
    {
        _findStat = GetComponent<FindStat>();
        _currentExp = _findStat.GetStat(StatsEnum.Experience);

    }

    private void Start()
    {
    }

    public float GetExperience()
    {
        return _currentExp;
    }

    public void ExperienceReward(float experience)
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

    public object CaptureState()
    {
        return _currentExp;
    }

    public void RestoreState(object state)
    {
        _currentExp = (float) state;
    }
}
