using System;
using System.Collections.Generic;
using System.Linq;
using Saving;
using UnityEngine;

namespace Quests
{
    public class QuestSystem : MonoBehaviour
    {
        [SerializeField] private Quest _quest;
        public Quest GetQuest => _quest;

        [SerializeField] private GameObject _uiCongratulations;
        [SerializeField] private GameObject _pointer;
        [SerializeField] private Canvas _mainCanvas;

        private List<Quest> _quests;
        public List<Quest> GetQuests => _quests;
        private void Awake()
        {
            _quests = new List<Quest>();
            _mainCanvas = GetComponentInChildren<Canvas>();
        }

        private void Update()
        {
            if (_quest == null)
            {
                _pointer.SetActive(false);
                return;
            }
            
            if(_quest.GetAim() == null) return;
            
            _pointer.transform.LookAt(_quest.GetAim());
        }

        private void Congratulations()
        {
            GameObject particleSystem = Instantiate(_uiCongratulations, _mainCanvas.transform);
            Destroy(particleSystem, 1f);
            _quests.Remove(_quest);
        }

        public void SetQuest(Quest quest)
        {
            if (_quest != null) return;
            
            _quest = quest;
            
            _quest.StartQuest();
            _pointer.SetActive(true);
            
            AddQuest(quest);
            QuestChanged();
        }
   
        public void AddQuest(Quest quest)
        {
           _quests.Add(quest);
        }
        
        private void QuestChanged()
        {
            _quest.OnSubquestCompleted += Congratulations;
            _quest.OnQuestCompleted += () =>
            {
                Congratulations();
                _quest = null;
            };
        }

    }
}