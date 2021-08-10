using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using UI.Cursor;
using UnityEngine;

public class QuestDialogueStrategy : InitializeDialogueStrategy
{
    private QuestSystem _questSystem;
    [SerializeField] private Quest _quest;
    [SerializeField] private GameObject _questBar;
    private void Awake()
    {
        _questSystem = FindObjectOfType<QuestSystem>();
    }

    public override event Action OnDialogChanged;

    public override void InitializeDialogMessage()
    {
        _questBar.SetActive(true);
        
        if(_quest == null) return;
             
        if (_questSystem.GetQuest == null)
        {
            _questSystem.SetQuest(_quest);
        }
        else
        {
            _questSystem.AddQuest(_quest);
        }
                
        _quest = null;
        
    }

}
