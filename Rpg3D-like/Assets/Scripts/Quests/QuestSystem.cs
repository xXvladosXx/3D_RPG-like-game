using System;
using UnityEngine;

namespace Quests
{
    public class QuestSystem : MonoBehaviour
    {
        [SerializeField] private Quest _quest;
        public Quest GetQuest => _quest;

        [SerializeField] private string _questState;
        [SerializeField] private GameObject _uiCongratulations;

        private Quest _secondaryQuest;
        
        private void OnEnable()
        {
            _quest.OnQuestCompleted += Congratulations;
        }

        private void OnDisable()
        {
            if (_quest.GetNextQuest != null)
            {
                SetQuest(_quest.GetNextQuest);
            }
        }

        private void Congratulations()
        {
            GameObject particleSystem = Instantiate(_uiCongratulations, gameObject.transform);
            Destroy(particleSystem, 1f);
            GetComponent<QuestSystem>().enabled = false;
        }

        public void SetQuest(Quest quest)
        {
            _quest = quest;
            GetComponent<QuestSystem>().enabled = true;
            quest.StartQuest();
        }

        
    }
}