using System;
using System.Collections.Generic;
using System.Linq;
using SavingSystem;
using UnityEngine;

namespace Quests
{
    public class QuestSystem : MonoBehaviour, ISaveable
    {
        [SerializeField] private Quest _quest;
        public Quest GetQuest => _quest;

        [SerializeField] private GameObject _uiCongratulations;
        [SerializeField] private GameObject _pointer;
        [SerializeField] private Canvas _mainCanvas;

        private List<Quest> _quests;
        public List<Quest> GetQuests => _quests;
        public event Action OnQuestChanged;
        private void Awake()
        {
            _quests = new List<Quest>();
            _mainCanvas = GameObject.FindWithTag("MainCanvas").GetComponent<Canvas>();
        }

        private void Update()
        {
            if (_quest == null)
            {
                _pointer.SetActive(false);
                return;
            }

            if (_quest.GetAim() == null)
            {
                _pointer.SetActive(false);
                return;
            }
            
            _pointer.transform.LookAt(_quest.GetAim());
        }

        private void Congratulations()
        {
            GameObject particleSystem = Instantiate(_uiCongratulations, _mainCanvas.transform);

            Destroy(particleSystem, 1f);
        }

        public void SetQuest(Quest quest)
        {
            if (_quest != null) return;

            _quest = quest;
            OnQuestChanged?.Invoke();

            QuestChanged();

            _quest.StartQuest();
            _pointer.SetActive(true);
            
            AddQuest(quest);
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
                _quests.Remove(_quest);
                _quest = null;
                OnQuestChanged?.Invoke();
            };
        }

        public object CaptureState()
        {
            return _quests.Select(quest => quest.name).ToList();
        }

        public void RestoreState(object state)
        {
            _quests.Clear();
            foreach (var questName in (List<string>)state)
            {   
                _quests.Add(Resources.Load<Quest>("Quests/" + questName));
            }
        }
    }
}