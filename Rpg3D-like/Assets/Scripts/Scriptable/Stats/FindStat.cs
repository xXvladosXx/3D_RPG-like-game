using System;
using System.Linq;
using SavingSystem;
using Stats;
using UnityEngine;

namespace Scriptable.Stats
{
    public class FindStat : MonoBehaviour, ISaveable
    {
        [SerializeField] private ProgressionScriptable _progression;
        [SerializeField] private CharactersEnum _charactersEnum;
        public CharactersEnum GetCharacterType => _charactersEnum;

        public event Action OnLevelUp;

        private LevelUp _levelUp;
        private int _startingLevel = 1;
        [SerializeField] private int _currentLevel = 1;

        private void Awake()
        {
            _levelUp = GetComponent<LevelUp>();
            _currentLevel = GetLevel();
        
            if (_levelUp != null)
            {
                _levelUp.OnExperienceGained += UpdateLevel;
            }
        }


        private void Start()
        {
            UpdateLevel();
            OnLevelUp?.Invoke();
        }

        public float GetStat(StatsEnum stat)
        {
            return (_progression.CalculateStat(stat, _charactersEnum, GetLevel()) + GetStatModifier(stat));
        }

        public int GetLevel()
        {
            if (_currentLevel < 1)
                _currentLevel = CalculateLevel();

            return _currentLevel;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if (newLevel > _currentLevel)
            {
                LevelUp levelUp = GetComponent<LevelUp>();

                _currentLevel = newLevel;
                levelUp.SpawnLevelUpEffect();

                OnLevelUp?.Invoke();
            }
        }

        public int CalculateLevel()
        {
            LevelUp levelUp = GetComponent<LevelUp>();

            if (levelUp == null) return _startingLevel;

            int maxLevel = _progression.GetLevels(StatsEnum.ExperienceToLevelUp, _charactersEnum);

            for (int level = 1; level < maxLevel; level++)
            {
                float expToLevelUp = _progression.CalculateStat(StatsEnum.ExperienceToLevelUp, _charactersEnum, level);

                if (expToLevelUp > levelUp.GetExperience())
                {
                    return level;
                }
            }

            return maxLevel + 1;
        }

        private float GetStatModifier(StatsEnum stat)
        {
            return GetComponents<IModifierStat>()
                .SelectMany(modifierStat => modifierStat.
                    GetStatModifier(stat))
                .Sum();
        }

        public object CaptureState()
        {
            return _charactersEnum;
        }

        public void RestoreState(object state)
        {
            _charactersEnum = (CharactersEnum) state;
        }
    }
}