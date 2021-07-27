using System;
using System.Linq;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest", order = 0)]
    
    public class Quest : ScriptableObject
    {
        [SerializeField] private InitializationQuest _mainQuest;
        public InitializationQuest GetCurrentQuest => _mainQuest;

        [SerializeField] private InitializationQuest[] _secondaryQuests;
        [SerializeField] private GameObject _npcQuestGiver;
        [SerializeField]
        public event Action OnQuestCompleted;
        public void StartQuest()
        {
            _npcQuestGiver = FindObjectOfType<DialogueMaking>().gameObject;

            _mainQuest.InitQuest(_npcQuestGiver, () =>
            {
                ContinueQuest(0);
            });
        }

        private void ContinueQuest(int index)
        {
            _mainQuest = null;
            if (index == _secondaryQuests.Length && _mainQuest == null)
            {
                OnQuestCompleted?.Invoke();
            }
            
            if(index > _secondaryQuests.Length - 1) return;
                
            _mainQuest = _secondaryQuests[index];
            
            OnQuestCompleted?.Invoke();
            
            _secondaryQuests[index].InitQuest(_npcQuestGiver, () => { ContinueQuest(index + 1); });
            
        }
    }
}