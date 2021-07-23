using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using UnityEngine;

public class QuestCheckerLocation : MonoBehaviour
{
    [SerializeField] private MoveToQuest _quest;
    public event Action OnLocationEntered;
    private QuestSystem _questSystem;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            if (_questSystem == null)
            {
                foreach (Transform child in playerController.GetComponentsInChildren<Transform>())
                {
                    if (child.GetComponent<QuestSystem>() != null)
                        _questSystem = child.GetComponent<QuestSystem>();
                }
            }
            
            if (_questSystem.GetQuest != null)
            {
                if (_questSystem.GetQuest.GetCurrentQuest != null && _questSystem.GetQuest.GetCurrentQuest == _quest)
                {
                    OnLocationEntered?.Invoke();
                }
            }
        }
    }
}
