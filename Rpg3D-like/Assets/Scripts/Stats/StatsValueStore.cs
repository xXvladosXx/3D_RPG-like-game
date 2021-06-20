using System;
using System.Collections.Generic;
using Interface;
using UnityEngine;
using UnityEngine.Rendering;

namespace Stats
{
    public class StatsValueStore : MonoBehaviour, IModifierStat
    {
        [SerializeField] private StatsBonus[] _statsBonus;
        [Serializable]
        class StatsBonus
        {
            public StatsEnum StatsEnum;
            public float modifiedBonus = 0;
        }

        public event Action OnStatsChanged;
        
        private Dictionary<StatsEnum, int> _assignedPoints = new Dictionary<StatsEnum, int>();
        private Dictionary<StatsEnum, int> _confirmedPoints = new Dictionary<StatsEnum, int>();

        private Dictionary<StatsEnum, Dictionary<StatsEnum, float>> _modifierBonusCondition;
        
        [SerializeField] private int _defaultNumberOfPoints = 2;

        private LevelUp _levelUp;
        
        private int _unassignedPoints = 10;
        public int GetUnassignedPoints => _unassignedPoints;

        private void Awake()
        {
            _modifierBonusCondition = new Dictionary<StatsEnum, Dictionary<StatsEnum, float>>();
            
            foreach (var statBonus in _statsBonus)
            {
                if (!_modifierBonusCondition.ContainsKey(statBonus.StatsEnum))
                {
                    _modifierBonusCondition[statBonus.StatsEnum] = new Dictionary<StatsEnum, float>();
                }

                _modifierBonusCondition[statBonus.StatsEnum][statBonus.StatsEnum] = statBonus.modifiedBonus;
            }
        }

        private void Start()
        {
            _levelUp = gameObject.GetComponent<LevelUp>();

            _levelUp.OnLevelUp += AddNewUnassignedPoints;
        }
        private void AddNewUnassignedPoints()
        {
            SetUnassignedPoints(_defaultNumberOfPoints);
        }
        public int GetProposedPoints(StatsEnum stat)
        {
            return GetPoints(stat) + GetConfirmedPoints(stat);
        }
        public int GetPoints(StatsEnum stat)
        {
            return _assignedPoints.ContainsKey(stat) ? _assignedPoints[stat] : 0;
        }

        public int GetConfirmedPoints(StatsEnum stat)
        {
            return _confirmedPoints.ContainsKey(stat) ? _confirmedPoints[stat] : 0;
        }

        public void SetUnassignedPoints(int points)
        {
            _unassignedPoints += points;
        }
        public void AssignPoints(StatsEnum stat, int points)
        {
            if(!CanAssignPoints(stat, points)) return;
            
            _confirmedPoints[stat] = GetConfirmedPoints(stat) + points;
            _unassignedPoints -= points;
        }

        public bool CanAssignPoints(StatsEnum stat, int points)
        {
            if (GetConfirmedPoints(stat) + points < 0) return false;
            if (_unassignedPoints < points) return false;
            
            return true;
        }

        public void Confirm()
        {
            foreach (var stat in _confirmedPoints.Keys)
            {
                _assignedPoints[stat] = GetProposedPoints(stat);
            }
            
            if (OnStatsChanged != null) OnStatsChanged();

            _confirmedPoints.Clear();
        }

        public IEnumerable<float> GetStatModifier(StatsEnum stat)
        {
            if(!_modifierBonusCondition.ContainsKey(stat)) yield break;

            foreach (StatsEnum statsEnum in _modifierBonusCondition[stat].Keys)
            {
                float bonus = _modifierBonusCondition[stat][statsEnum];
                yield return bonus + GetPoints(statsEnum);
            }
        }
    }
}