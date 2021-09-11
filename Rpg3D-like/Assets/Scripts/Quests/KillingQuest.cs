using System;
using System.Collections.Generic;
using System.Linq;
using Scriptable.Weapon;
using Stats;
using TMPro;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "KillingQuest", menuName = "Quests/KillingQuest", order = 0)]
    public class KillingQuest : InitializationQuest
    {
        [SerializeField] private int _questAmountKill;

        private GameObject _player;

        public override void InitQuest(Action completed)
        {
            _player = GameObject.FindWithTag("Player");

            List<CombatTarget> combatTargets = new List<CombatTarget>();
            foreach (var combatTarget in FindObjectsOfType<CombatTarget>())
            {
                if (combatTarget.gameObject.CompareTag(QuestAim))
                    combatTargets.Add(combatTarget);
            }

            ComplicatedKillingQuest killingQuest = new ComplicatedKillingQuest();
            killingQuest.Kill(combatTargets, _questAmountKill);
            
            killingQuest.OnUpdateQuest += () =>
            {
                _player.GetComponent<LevelUp>().ExperienceReward(Experience);

                GameObject.FindGameObjectWithTag("QuestName").GetComponent<TextMeshProUGUI>().text = "None";
                completed();
            };

        }

        public override GameObject GetAim()
        {
            CombatTarget[] enemies = FindObjectsOfType<CombatTarget>();

            foreach (var enemy in enemies)
            {
                if (enemy.gameObject.CompareTag(QuestAim))
                    return enemy.gameObject;
            }

            return null;
        }
    }
}