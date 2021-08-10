using System;
using System.Linq;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quests/Quest", order = 0)]
    
    public class Quest : ScriptableObject
    {
        [SerializeField] private InitializationQuest _mainQuest;
        [SerializeField] private InitializationQuest[] _secondaryQuests;
        [SerializeField] private GameObject _npcQuestGiver;
        [SerializeField]
        public event Action OnQuestCompleted;
        public event Action OnSubquestCompleted;
        public void StartQuest()
        {
            _mainQuest.InitQuest(() =>
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
            
            OnSubquestCompleted?.Invoke();
            
            _secondaryQuests[index].InitQuest(() => { ContinueQuest(index + 1); });
            
        }

        public Transform GetAim()
        {
            if (_mainQuest == null) return null;
            
            return _mainQuest.GetAim().transform;
        }
    }
}