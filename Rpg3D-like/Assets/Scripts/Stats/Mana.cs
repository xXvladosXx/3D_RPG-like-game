using System;
using System.Collections.Generic;
using System.IO;
using Interface;
using UnityEngine;
using UnityEngine.AI;

namespace Stats
{
    public class Mana : MonoBehaviour
    { 
        public event Action OnSkillCast;
        
        private FindStat _findStat;
        [SerializeField] private float _manaCurrent;
        [SerializeField] float _manaMax;
        [SerializeField] private float _manaSpeedRegeneration = 0.5f;
        private StatsValueStore _statsValueStore;

        private void Awake()
        {
            _findStat = GetComponent<FindStat>();
            
            GetComponent<FindStat>().OnLevelUp += SetNewLevelMana;
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
            _manaMax = _findStat.GetStat(StatsEnum.Mana);
            _manaCurrent = _manaMax;
        }

        public float GetCurrentMana()
        {
            return _manaCurrent;
        }

        public float GetFraction()
        {
            return _manaCurrent / _manaMax;
        }

        public void CasteSkill(float manaPoints)
        {
            _manaCurrent -= manaPoints;
            if (OnSkillCast != null) OnSkillCast();
        }

        private void ManaRegeneration()
        {
            if (_manaCurrent < _manaMax)
            {
                _manaCurrent += Time.deltaTime*_manaSpeedRegeneration;
                if (_manaCurrent > _manaMax)
                    _manaCurrent = _manaMax;
            }
        }
    }
}