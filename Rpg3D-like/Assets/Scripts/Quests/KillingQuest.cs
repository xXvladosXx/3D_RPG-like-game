using System;
using System.Collections.Generic;
using System.Linq;
using Scriptable.Weapon;
using TMPro;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "KillingQuest", menuName = "Quests/KillingQuest", order = 0)]
    public class KillingQuest : InitializationQuest
    {
        [SerializeField] private GameObject _npc; 
        [SerializeField] private string _questName;
        [SerializeField] private float _experienceAmount;
        [SerializeField] private string _questAim;
        [SerializeField] private int _questAmountKill;

        private GameObject _player;

        public override void InitQuest(Action completed)
        {
            _player = GameObject.FindWithTag("Player");

            GameObject.FindGameObjectWithTag("QuestName").GetComponent<TextMeshProUGUI>().text = _questName;
            TextMeshProUGUI amountToKill = GameObject.FindGameObjectWithTag("QuestAmountToKill").GetComponent<TextMeshProUGUI>();
            amountToKill.enabled = true;
            amountToKill.text = _questAmountKill.ToString();

            TextMeshProUGUI enemyToKill = GameObject.FindGameObjectWithTag("QuestAim").GetComponent<TextMeshProUGUI>();
            enemyToKill.enabled = true;
            enemyToKill.text = _questAim;
            
            List<CombatTarget> combatTargets = new List<CombatTarget>();
            foreach (var combatTarget in FindObjectsOfType<CombatTarget>())
            {
                if (combatTarget.gameObject.CompareTag(_questAim))
                    combatTargets.Add(combatTarget);
            }

            ComplicatedKillingQuest killingQuest = new ComplicatedKillingQuest();
            killingQuest.Kill(combatTargets, _questAmountKill);
            
            killingQuest.OnUpdateQuest += () =>
            {
                _player.GetComponent<LevelUp>().ExperienceReward(_experienceAmount);

                GameObject.FindGameObjectWithTag("QuestName").GetComponent<TextMeshProUGUI>().text = "None";
                amountToKill.enabled = false;
                enemyToKill.enabled = false;
                completed();
            };

        }

        public override GameObject GetAim()
        {
            CombatTarget[] enemies = FindObjectsOfType<CombatTarget>();

            foreach (var enemy in enemies)
            {
                if (enemy.gameObject.CompareTag(_questAim))
                    return enemy.gameObject;
            }

            return null;
        }
    }
}