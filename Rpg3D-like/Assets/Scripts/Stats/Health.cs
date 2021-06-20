using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Health : MonoBehaviour, IAction
{
    [SerializeField] public OnTakeDamageEventArgs OnDamageTaken;
    
    public event Action OnTakeDamage;
    public event Action OnTakeHealing;
    
    private ActionScheduler _actionScheduler;
    private FindStat _findStat;
    [SerializeField] private float _healthCurrent;
    [SerializeField] private float _healthMax;
    private Animator _animator;

    [Serializable]
    public class OnTakeDamageEventArgs : UnityEvent<float>
    {
    }

    private void Awake()
    {
        _findStat = GetComponent<FindStat>();
        _actionScheduler = GetComponent<ActionScheduler>();
        _animator = GetComponent<Animator>();
        
        GetComponent<LevelUp>().OnLevelUp += SetNewLevelHealth;
    }

    private void Start()
    {
        _healthMax = _findStat.GetStat(StatsEnum.Health);

        _healthCurrent = _healthMax;
    }
    public void SetNewLevelHealth()
    {
        _healthMax = _findStat.GetStat(StatsEnum.Health);
        _healthCurrent = _healthMax;
    }

    public float GetCurrentHealth()
    {
        return _healthCurrent;
    }

    public bool IsDead()
    {
        return _healthCurrent <= 0;
    }

    public void Death()
    {
        Destroy(this);
        GetComponent<CombatTarget>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        Destroy(GetComponent<Rigidbody>());
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

        if (gameObject.GetComponent<PlayerController>() != null)
        {
            
        }
    }

    public void Cancel()
    {
        _actionScheduler.Cancel();
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
}
