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

        [SerializeField] private InitializationQuest _initQuest;
        [SerializeField] private GameObject _uiCongratulations;
        [SerializeField] private GameObject _pointer;
        [SerializeField] private Canvas _mainCanvas;

        private List<Quest> _quests;
        public List<Quest> GetQuests => _quests;
        private void Awake()
        {
            _quests = new List<Quest>();
            _mainCanvas = GameObject.FindWithTag("MainCanvas").GetComponent<Canvas>();
        }

        private void OnEnable()
        {
            if(_quest == null) return;
            _quest.OnQuestCompleted += Congratulations;
        }

        private void Update()
        {
            if (_initQuest == null)
            {
                _pointer.SetActive(false);
                _quest = null;
                return;
            }
            
            if(_initQuest.GetAim() == null ) return;
            
            _pointer.SetActive(true);
            _pointer.transform.LookAt(_initQuest.GetAim().transform);
        }

        private void Congratulations()
        {
            GameObject particleSystem = Instantiate(_uiCongratulations, _mainCanvas.transform);
            Destroy(particleSystem, 1f);
            
            QuestChanged();
            
            _quests.Remove(_quest);
        }

        public void SetQuest(Quest quest)
        {
            if(quest == _quest) return;
            
            _quest = quest;
            
            GetComponent<QuestSystem>().enabled = true;
            
            quest.StartQuest();
            QuestChanged();
        }
   
        public void AddQuest(Quest quest)
        {
            if (!_quests.Contains(quest))
                _quests.Add(quest);
        }
        
        private void QuestChanged()
        {
            _initQuest = _quest.GetCurrentQuest;
            _quest.OnQuestCompleted += Congratulations;
        }

    }
}