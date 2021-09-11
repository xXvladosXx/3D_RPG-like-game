using System;
using System.Collections.Generic;
using System.IO;
using Scriptable.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace Stats
{
    public class Mana : MonoBehaviour
    {
        public float ManaCurrent;
        public float ManaMax;
        
        [SerializeField] private float _manaSpeedRegeneration = 1.5f;
        
        private FindStat _findStat;
        private StatsValueStore _statsValueStore;
        public event Action OnManaChanged;

        private void Awake()
        {
            _findStat = GetComponent<FindStat>();
            _statsValueStore = GetComponent<StatsValueStore>();

            GetComponent<FindStat>().OnLevelUp += SetNewLevelMana;
            
            if (_statsValueStore != null)
            {
                _statsValueStore.OnStatsChanged += () =>
                {
                    ManaMax = _findStat.GetStat(StatsEnum.Mana);
                    OnManaChanged?.Invoke();
                };
            } 
        }

        private void Start()
        {
            if (_statsValueStore != null)
            {
                _statsValueStore.OnStatsChanged += SetNewLevelMana;
            }

            SetNewLevelMana();
        }

        private void Update()
        {
            ManaRegeneration();
        }

        public void SetNewLevelMana()
        {
            ManaMax = _findStat.GetStat(StatsEnum.Mana);
            ManaCurrent = ManaMax;
        }

        public float GetCurrentMana()
        {
            return ManaCurrent;
        }

        public float GetFraction()
        {
            return ManaCurrent / ManaMax;
        }

        public void CasteSkill(float manaPoints)
        {
            ManaCurrent -= manaPoints;
        }

        private void ManaRegeneration()
        {
            if (!(ManaCurrent < ManaMax)) return;
            
            ManaCurrent += Time.deltaTime*_manaSpeedRegeneration;
            if (ManaCurrent > ManaMax)
                ManaCurrent = ManaMax;
        }

        public void RegenerateMana(float regeneration)
        {
            ManaCurrent = Mathf.Min(ManaCurrent + regeneration, ManaMax);
        }
    }
}