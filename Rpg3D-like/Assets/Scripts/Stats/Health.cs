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
    
    public event Action OnTakeDamage;
    public event Action OnTakeHealing;
    
    [SerializeField] private float _healthCurrent;
    [SerializeField] private float _healthMax;
    
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
        _findStat.OnLevelUp += SetNewLevelHealth;
        _healthMax = _findStat.GetStat(StatsEnum.Health);
    }

    private void Start()
    {
        if (_statsValueStore != null)
        {
            _statsValueStore.OnStatsChanged += SetNewLevelHealth;
        }
    }

    private void Update()
    {
    }

    public void SetNewLevelHealth()
    {
        _healthMax = _findStat.GetStat(StatsEnum.Health);
        _healthCurrent = _healthMax;
    }

    public bool IsDead()
    {
        return _healthCurrent <= 0;
    }

    public void Death()
    {
        GetComponent<CombatTarget>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        Destroy(GetComponent<Rigidbody>());
        GetComponent<NavMeshAgent>().obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        _actionScheduler.Cancel();
        _animator.SetTrigger("isDead");
    }
    
    public float GetFraction()
    {
        return _healthCurrent / _healthMax;
    }

    public void TakeDamage(float damage, GameObject damager)
    {
        OnDamageTaken.Invoke(damage);
        
        _healthCurrent -= damage;

        if (IsDead())
        {
            damager.GetComponent<LevelUp>().CalculateExperience(GetComponent<FindStat>().GetStat(StatsEnum.ExperienceReward)); 
            Death();
        }

        if (OnTakeDamage != null) OnTakeDamage();
    }

    public void RegenerateHealth()
    {
        if (_healthCurrent < _healthMax)
        {
            _healthCurrent += (_healthMax/100)*10;

            if (_healthCurrent > _healthMax)
                _healthCurrent = _healthMax;
        }

        if (OnTakeHealing != null) OnTakeHealing();
    }

    public void RegenerateHealthFromSpell(float healing)
    {
        _healthCurrent += healing;

        if (_healthCurrent > _healthMax)
            _healthCurrent = _healthMax;
        
        if (OnTakeHealing != null) OnTakeHealing();
    }

    public object CaptureState()
    {
        return _healthCurrent;
    }

    public void RestoreState(object state)
    {
        _healthCurrent = (float)state;
        
        if(_healthCurrent <= 0)
            Death();
    }
}
