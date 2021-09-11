using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable.Stats
{
    [CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObjects/Stats", order = 1)]
    public class ProgressionScriptable : ScriptableObject
    {
        [SerializeField] private ProgressionCharactersClass[] _progressionCharactersClasses;

        private Dictionary<CharactersEnum, Dictionary<StatsEnum, float[]>> _lookThroughCharactersStats;

        public float CalculateStat(StatsEnum stat, CharactersEnum character, int level)
        {
            BuildTable();
        
            float[] levels = _lookThroughCharactersStats[character][stat];

            if (levels.Length <= 0)
                return 0;

            return levels[level-1];
        }
    
        public int GetLevels(StatsEnum stat, CharactersEnum character)
        {
            BuildTable();
        
            float[] levels = _lookThroughCharactersStats[character][stat];
            return levels.Length;
        }

        private void BuildTable()
        {
            if(_lookThroughCharactersStats != null) return;

            _lookThroughCharactersStats = new Dictionary<CharactersEnum, Dictionary<StatsEnum, float[]>>();

            foreach (ProgressionCharactersClass progressionCharactersClass in _progressionCharactersClasses)
            {
                var statTable = new Dictionary<StatsEnum, float[]>();

                foreach (var progressionStats in progressionCharactersClass.stats)
                {
                    statTable[progressionStats.stat] = progressionStats.levels;
                }

                _lookThroughCharactersStats[progressionCharactersClass.characterClass] = statTable;
            }
        
        }
    
        [Serializable]
        class ProgressionCharactersClass
        {
            public CharactersEnum characterClass;
            public ProgressionStats[] stats;
        }
    
        [Serializable]
        class ProgressionStats
        {
            public StatsEnum stat;
            public float[] levels;
        }
    }
}
