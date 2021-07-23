using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quests
{
    public class QuestSystem : MonoBehaviour
    {
        [SerializeField] private Quest _quest;
        public Quest GetQuest => _quest;

        [SerializeField] private string _questState;
        [SerializeField] private GameObject _uiCongratulations;
        [SerializeField] private GameObject _pointer;

        private List<Quest> _quests;
        
        private void OnEnable()
        {
            _quest.OnQuestCompleted += Congratulations;
        }

        private void Update()
        {
            if(GetQuest.GetCurrentQuest.GetAim() == null) return;
            
            _pointer.SetActive(true);
            _pointer.transform.LookAt(GetQuest.GetCurrentQuest.GetAim().transform);
        }

        private void Congratulations()
        {
            GameObject particleSystem = Instantiate(_uiCongratulations, gameObject.transform);
            Destroy(particleSystem, 1f);
            
            GetComponent<QuestSystem>().enabled = false;

            if (_quest != null)
            {
                if (_quest.GetNextQuest != null)
                {
                    SetQuest(_quest.GetNextQuest);
                }
                else
                {
                    _pointer.SetActive(false);
                    _quest = null;
                }
            }
        }

        public void SetQuest(Quest quest)
        {
            _quests.Add(quest);
            _quest = quest;
            GetComponent<QuestSystem>().enabled = true;
            quest.StartQuest();
        }

        
    }
}