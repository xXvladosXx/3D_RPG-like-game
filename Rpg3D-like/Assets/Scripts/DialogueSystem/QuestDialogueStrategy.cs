using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using UnityEngine;

public class QuestDialogueStrategy : InitializeDialogueStrategy
{
    private QuestSystem _questSystem;
    [SerializeField] private Quest initializationQuest;
    [SerializeField] private GameObject _questBar;
    private void Awake()
    {
        _questSystem = FindObjectOfType<QuestSystem>();
    }
    
    public override void InitializeDialogMessage()
    {
        _questBar.SetActive(true);
        
        if(initializationQuest == null) return;
             
        if (_questSystem.GetQuest == null)
        {
            _questSystem.SetQuest(initializationQuest);
            _questSystem.AddQuest(initializationQuest);
        }
        else
        {
            _questSystem.AddQuest(initializationQuest);
        }
                
        initializationQuest = null;
        
    }
}
