using System;
using System.Globalization;
using Quests;
using TMPro;
using UnityEngine;

namespace UI.QuestBar
{
    public class QuestBarUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _questName;
        [SerializeField] private TextMeshProUGUI _experienceAmount;
        [SerializeField] private TextMeshProUGUI _questAim;
        [SerializeField] private QuestSystem _questSystem;

        private void Start()
        {
            _questSystem.OnQuestChanged += UpdateInfoQuest;
        }

        private void UpdateInfoQuest()
        {
            if (_questSystem.GetQuest == null)
            {
                _questName.text = "None";
                _questAim.text = "";
                _experienceAmount.text = "";
                
                return;
            }
            
            _questName.text = _questSystem.GetQuest.GetCurrentQuest.QuestName;
            _questAim.text = _questSystem.GetQuest.GetCurrentQuest.QuestAim;
            _experienceAmount.text = _questSystem.GetQuest.GetCurrentQuest.Experience.ToString(CultureInfo.InvariantCulture);
        }
    }
}