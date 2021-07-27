using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Quests
{
    public class QuestSetter : MonoBehaviour
    {
        [SerializeField] private QuestSystem _questSystem;
        [SerializeField] private Transform _content;
        [SerializeField] private GameObject _questPrefab;
        private void Awake()
        {
            _questSystem = FindObjectOfType<QuestSystem>();
        }

        private void OnEnable()
        {
            if(_questSystem == null) return;
            
            foreach (Transform child in _content)
            {
                Destroy(child.gameObject);
            }
            
            if(_questSystem.GetQuests == null) return;
            
            foreach (var quest in _questSystem.GetQuests)
            {
                GameObject questPrefab = Instantiate(_questPrefab, _content);
            
                Button questButton = questPrefab.GetComponent<Button>();

                questButton.GetComponentInChildren<TextMeshProUGUI>().text = quest.name;
                questButton.onClick.AddListener((() => { _questSystem.SetQuest(quest); }));
            }
        }
    }
}