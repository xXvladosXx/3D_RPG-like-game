using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindStat : MonoBehaviour
{
    [SerializeField] private ProgressionScriptable _progression;
    [SerializeField] private CharactersEnum _charactersEnum;

    private LevelUp _levelUp;
    private void Awake()
    {
        _levelUp = GetComponent<LevelUp>();
    }

    public float GetStat(StatsEnum stat)
    {
        return _progression.CalculateStat(stat, _charactersEnum, _levelUp.GetLevel());
    }

    public int CalculateLevel()
        {
            LevelUp levelUp = GetComponent<LevelUp>();
    
            if (levelUp == null) return _levelUp.GetLevel();
    
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

}
