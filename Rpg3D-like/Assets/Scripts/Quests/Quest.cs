using System;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] private Quest _secondaryQuest;
        public Quest GetNextQuest => _secondaryQuest;
        
        [SerializeField] private InitializationQuest _mainQuest;
        public InitializationQuest GetCurrentQuest => _mainQuest;
        
        [SerializeField] private GameObject _npcQuestGiver;

        public event Action OnQuestCompleted;
        public void StartQuest()
        {
            _npcQuestGiver = FindObjectOfType<DialogMessage>().gameObject;

            _mainQuest.InitQuest(_npcQuestGiver, () =>
            {
                ContinueQuest();
            });
        }

        private void ContinueQuest()
        {
            OnQuestCompleted?.Invoke();
            
            if(_secondaryQuest!=null)
                _secondaryQuest.StartQuest();
        }
    }
}