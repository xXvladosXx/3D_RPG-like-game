using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using UnityEngine;

public class QuestCheckerLocation : MonoBehaviour
{
    [SerializeField] private Quest _quest;
    public event Action OnLocationEntered;
    private QuestSystem _questSystem;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            if (_questSystem == null)
            {
                _questSystem = FindObjectOfType<QuestSystem>();
            }
            
            if (_questSystem.GetQuest != null)
            {
                if (_questSystem.GetQuest == _quest)
                {
                    OnLocationEntered?.Invoke();
                }
            }
        }
    }
}
