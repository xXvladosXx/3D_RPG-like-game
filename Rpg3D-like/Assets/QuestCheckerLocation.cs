using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using UnityEngine;

public class QuestCheckerLocation : MonoBehaviour
{
    [SerializeField] private MoveToQuest _quest;

    private bool _interactionWasPerfomed = false;
    public event Action OnLocationEntered;
    QuestSystem questSystem;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            if (questSystem == null)
            {
                foreach (Transform child in playerController.GetComponentsInChildren<Transform>())
                {
                    if (child.GetComponent<QuestSystem>() != null)
                        questSystem = child.GetComponent<QuestSystem>();
                }
            }

            if (!_interactionWasPerfomed)
            {
                _interactionWasPerfomed = true;
                OnLocationEntered?.Invoke();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
}
