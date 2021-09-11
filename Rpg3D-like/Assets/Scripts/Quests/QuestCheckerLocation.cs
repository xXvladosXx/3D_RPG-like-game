using System;
using Controller;
using UnityEngine;

namespace Quests
{
    public class QuestCheckerLocation : MonoBehaviour
    {
        [SerializeField] private InitializationQuest _quest;
        public event Action OnLocationEntered ;
        private QuestSystem _questSystem;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController playerController))
            {
                if (_questSystem == null)
                {
                    _questSystem = FindObjectOfType<QuestSystem>();
                }
            
                if (_questSystem.GetQuest == null) return;
                if(_questSystem.GetQuest.GetCurrentQuest == null) return;
                if (_questSystem.GetQuest.GetCurrentQuest != _quest) return;
            
                OnLocationEntered?.Invoke();
            }
        }
    }
}
