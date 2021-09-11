using System;
using System.Collections;
using System.Collections.Generic;
using Interface;
using Saving;
using Stats;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Health : MonoBehaviour, ISaveable
{
    [SerializeField] public OnTakeDamageEventArgs OnDamageTaken;

    public event Action<GameObject> OnTakeDamage;
    public event Action OnTakeHealing;
    public event Action OnDied;

    [SerializeField] private float _healthCurrent;
    [SerializeField] private float _healthMax;

    private bool _wasDead = false;
    private FindStat _findStat;
    private Animator _animator;
    private StatsValueStore _statsValueStore;
    private ActionScheduler _actionScheduler;

    [Serializable]
    public class OnTakeDamageEventArgs : UnityEvent<float>
    {
    }

    private void Awake()
    {
        _actionScheduler = GetComponent<ActionScheduler>();
        _findStat = GetComponent<FindStat>();
        _animator = GetComponent<Animator>();
        _statsValueStore = GetComponent<StatsValueStore>();
        SetNewLevelHealth();
        
        if (_statsValueStore != null)
        {
            _statsValueStore.OnStatsChanged += SetNewLevelHealth;
        }
        _findStat.OnLevelUp += SetNewLevelHealth;
    }


    public void SetNewLevelHealth()
    {
        _healthMax = _findStat.GetStat(StatsEnum.Health);
        _healthCurrent = _healthMax;
    }

    public float GetFraction()
    {
        return _healthCurrent / _healthMax;
    }

    public void TakeDamage(float damage, GameObject damager)
    {
        OnDamageTaken.Invoke(damage);
        
        _healthCurrent = Mathf.Max(_healthCurrent - damage, 0);

        if (IsDead())
        {
            damager.GetComponent<LevelUp>().ExperienceReward(GetComponent<FindStat>().GetStat(StatsEnum.ExperienceReward)); 
            OnDied?.Invoke();
            Death();
        }
        else
        {
            OnTakeDamage?.Invoke(damager);
        }

    }

    public bool IsDead()
    {
        return _healthCurrent == 0;
    }

    private void Death()
    {
        if (!_wasDead && IsDead())
        {
            _actionScheduler.Cancel();
            _animator.SetTrigger("isDead");
        }
        
        if(_wasDead && !IsDead())
        {
            _animator.Rebind();
        }

        _wasDead = IsDead();
    }

    
    public void RegenerateHealth(float healing)
    {
        _healthCurrent = Mathf.Min(_healthCurrent + healing, _healthMax);
        
        OnTakeHealing?.Invoke();
    }

    public object CaptureState()
    {
        return _healthCurrent;
    }

    public void RestoreState(object state)
    {
        _healthCurrent = (float)state;
      
        Death();
    }

    
}
