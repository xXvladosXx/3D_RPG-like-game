using System;
using System.Collections.Generic;
using SavingSystem;
using Scriptable.Stats;
using UnityEngine;
using UnityEngine.Rendering;

namespace Stats
{
    public class StatsValueStore : MonoBehaviour, IModifierStat, ISaveable
    {
        [SerializeField] private SerializableDictionary<StatsEnum, StatBonus> _statsBonus;
        
        [Serializable]
        public class StatBonus
        {
            public StatsModifier StatsModifier;
            public float Bonus;
        }
        public event Action OnStatsChanged;
        
        private Dictionary<StatsModifier, int> _assignedPoints = new Dictionary<StatsModifier, int>();
        private Dictionary<StatsModifier, int> _confirmedPoints = new Dictionary<StatsModifier, int>();

        [SerializeField] private int _defaultNumberOfPoints = 2;

        private FindStat _findStat;
        
        private int _unassignedPoints = 10;
        public int GetUnassignedPoints => _unassignedPoints;

        private void Awake()
        {
            _findStat = gameObject.GetComponent<FindStat>();

            _findStat.OnLevelUp += AddNewUnassignedPoints;
        }

        private void AddNewUnassignedPoints()
        {
            SetUnassignedPoints(_defaultNumberOfPoints);
        }
        public int GetProposedPoints(StatsModifier stat)
        {
            return GetPoints(stat) + GetConfirmedPoints(stat);
        }
        public int GetPoints(StatsModifier stat)
        {
            return _assignedPoints.ContainsKey(stat) ? _assignedPoints[stat] : 0;
        }

        public int GetConfirmedPoints(StatsModifier stat)
        {
            return _confirmedPoints.ContainsKey(stat) ? _confirmedPoints[stat] : 0;
        }

        public void SetUnassignedPoints(int points)
        {
            _unassignedPoints += points;
        }
        public void AssignPoints(StatsModifier stat, int points)
        {
            if(!CanAssignPoints(stat, points)) return;
            
            _confirmedPoints[stat] = GetConfirmedPoints(stat) + points;
            _unassignedPoints -= points;
        }

        public bool CanAssignPoints(StatsModifier stat, int points)
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
            
            OnStatsChanged?.Invoke();

            _confirmedPoints.Clear();
        }

        public IEnumerable<float> GetStatModifier(StatsEnum stat)
        {
            if(_statsBonus.TryGetValue(stat, out var s))
            {
                yield return GetPoints(s.StatsModifier) * s.Bonus;
            }
        }
        

        public object CaptureState()
        {
            return _assignedPoints;
        }

        public void RestoreState(object state)
        {
            _assignedPoints = new Dictionary<StatsModifier, int>((Dictionary<StatsModifier, int>) state);
        }
    }
}