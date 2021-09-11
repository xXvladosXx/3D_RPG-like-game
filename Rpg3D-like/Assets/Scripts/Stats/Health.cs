using System;
using Controller;
using Resistance;
using SavingSystem;
using Scriptable.Stats;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Stats
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] public OnTakeDamageEventArgs OnDamageTaken;

        public event Action<GameObject> OnTakeDamage;
        public event Action OnHealthChanged;
        public event Action OnDied;

        public float HealthCurrent;
        public float HealthMax;

        private bool _wasDead = false;
        private FindStat _findStat;
        private Animator _animator;
        private StatsValueStore _statsValueStore;
        private ActionScheduler _actionScheduler;
        private Armour _armour;
        private Equipment _equipment;
        private float _healthRegeneration;


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
            _armour = GetComponent<Armour>();
            _equipment = GetComponent<Equipment>();
        
          
            SetNewLevelHealth();
        
            if (_statsValueStore != null)
            {
                _statsValueStore.OnStatsChanged += () =>
                {
                    HealthMax = _findStat.GetStat(StatsEnum.Health);
                    _healthRegeneration = _findStat.GetStat(StatsEnum.HealthRegeneration);
                
                    print(_healthRegeneration);
                    OnHealthChanged?.Invoke();
                };
            } 
        
            _findStat.OnLevelUp += SetNewLevelHealth;
        
            if(_equipment != null )
            {
                _equipment.OnEquipmentChanged += () =>
                {
                    HealthMax = _findStat.GetStat(StatsEnum.Health);
                    HealthCurrent = Mathf.Clamp(HealthCurrent, 0, HealthMax);
                    OnHealthChanged?.Invoke();
                };
            }
        }

        private void SetNewLevelHealth()
        {
            HealthMax = _findStat.GetStat(StatsEnum.Health);
            HealthCurrent = HealthMax;
        }

        public float GetFraction()
        {
            return HealthCurrent / HealthMax;
        }

        public void TakeDamage(float damage, GameObject damager, DamageType damageType)
        {
            var ownResistance = _armour.GetDamageResistance;
        
            if (ownResistance != null)
            {
                damage = ownResistance.CalculateResistance(damage, damageType);
                damage += ownResistance.CalculateResistance(_findStat.GetStat(StatsEnum.Damage), DamageType.Physical);
            }

            HealthCurrent = Mathf.Clamp(HealthCurrent - damage, 0, HealthMax);
            OnDamageTaken.Invoke(damage);
        
            if (IsDead())
            {
                damager.GetComponent<LevelUp>().ExperienceReward(GetComponent<FindStat>().GetStat(StatsEnum.ExperienceReward)); 
                OnHealthChanged?.Invoke();
                OnDied?.Invoke();
                Death();
            }
            else
            {
                OnTakeDamage?.Invoke(damager);
                OnHealthChanged?.Invoke();
            }
        }
    
        public void TakeDamage(SerializableDictionary<DamageType, float> damagePairs, GameObject damager)
        {
            var ownResistance = _armour.GetDamageResistance;
            float damage = 0;
        
            foreach (var damagePair in damagePairs)
            {
                damage += ownResistance.CalculateResistance(damagePair.Value, damagePair.Key);
            }

            damage += ownResistance.CalculateResistance(damager.GetComponent<FindStat>().GetStat(StatsEnum.Damage), DamageType.Physical);
            HealthCurrent = Mathf.Clamp(HealthCurrent - damage, 0, HealthMax);
            OnDamageTaken.Invoke(damage);
        
            if (IsDead())
            {
                damager.GetComponent<LevelUp>().ExperienceReward(GetComponent<FindStat>().GetStat(StatsEnum.ExperienceReward)); 
                OnHealthChanged?.Invoke();
                OnDied?.Invoke();
                Death();
            }
            else
            {
                OnTakeDamage?.Invoke(damager);
                OnHealthChanged?.Invoke();
            }
        }

        public bool IsDead()
        {
        
            return HealthCurrent == 0;
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
            HealthCurrent = Mathf.Min(HealthCurrent + healing, HealthMax);
        
            OnHealthChanged?.Invoke();
        }

        public object CaptureState()
        {
            return HealthCurrent;
        }

        public void RestoreState(object state)
        {
            HealthCurrent = (float)state;
      
            Death();
        }

    
    }
}
