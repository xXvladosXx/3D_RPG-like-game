using System;
using System.Collections;
using System.Collections.Generic;
using Saving;
using UnityEngine;

public class LevelUp : MonoBehaviour, ISaveable
{
    [SerializeField] private float _currentExp;
    public event Action OnExperienceGained;
    
    [SerializeField] private ParticleSystem _levelUpEffect;

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            ExperienceReward(Time.deltaTime*10);
        }
    }

    public float GetExperience()
    {
        return _currentExp;
    }

    public void ExperienceReward(float experience)
    {
        _currentExp += experience;
        OnExperienceGained?.Invoke();
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
