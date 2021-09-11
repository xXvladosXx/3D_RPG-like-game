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
            if (index == _secondaryQuests.Length)
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
            if (_mainQuest.GetAim() == null) return null;
            
            return _mainQuest.GetAim().transform;
        }
    }
}