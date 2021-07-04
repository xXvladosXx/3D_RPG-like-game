using System;
using System.Collections;
using System.Collections.Generic;
using Interface;
using UnityEngine;

public class FindStat : MonoBehaviour
{
    [SerializeField] private ProgressionScriptable _progression;
    [SerializeField] private CharactersEnum _charactersEnum;
    public event Action OnLevelUp;

    private LevelUp _levelUp;
    private int _startingLevel = 1;
    [SerializeField] private int _currentLevel = 1;
    private void Awake()
    {
        _levelUp = GetComponent<LevelUp>();

        if (_levelUp != null)
        {
            _levelUp.OnExperienceGained += UpdateLevel;
        }
    }

    private void Start()
    {
        _currentLevel = GetLevel();
    }

    public float GetStat(StatsEnum stat)
    {
        return _progression.CalculateStat(stat, _charactersEnum, GetLevel()) + GetStatModifier(stat);
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

            if (OnLevelUp != null) OnLevelUp();
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

    public float GetStatModifier(StatsEnum stat)
    {
        float total = 0;

        foreach (IModifierStat modifierStat in GetComponents<IModifierStat>())
        {
            foreach (float modifier in modifierStat.GetStatModifier(stat))
            {
                total += modifier;
            }            
        }

        return total;
    }
}
