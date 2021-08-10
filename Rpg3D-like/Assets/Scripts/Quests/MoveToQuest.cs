using System;
using TMPro;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "MovingToQuest", menuName = "Quests/MovingQuest", order = 0)]
    public class MoveToQuest : InitializationQuest
    {
        [SerializeField] private GameObject _questAim;
        [SerializeField] private float _experience;
        [SerializeField] private string _questName;
        
        public override void InitQuest(Action completed)
        {
            GameObject player = GameObject.FindWithTag("Player");

            QuestCheckerLocation questCheckerLocation = GameObject.Find(_questAim.name).GetComponent<QuestCheckerLocation>();
            GameObject.FindGameObjectWithTag("QuestName").GetComponent<TextMeshProUGUI>().text = _questName;
            TextMeshProUGUI questAim = GameObject.FindGameObjectWithTag("QuestAim").GetComponent<TextMeshProUGUI>();
            questAim.enabled = true;
            questAim.text = _questAim.name;

            SimpleMovingQuest simpleMovingQuest = new SimpleMovingQuest();
            simpleMovingQuest.MoveTo(GameObject.FindWithTag("Player"), _questAim);
            
            questCheckerLocation.OnLocationEntered += () =>
            {
                GameObject.FindGameObjectWithTag("QuestName").GetComponent<TextMeshProUGUI>().text = "None";
                player.GetComponent<LevelUp>().ExperienceReward(_experience);
                questAim.enabled = false;
                completed();
            };
        }

        public override GameObject GetAim()
        {
            return _questAim;
        }
    }
}