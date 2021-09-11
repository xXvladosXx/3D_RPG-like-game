using System;
using System.Collections.Generic;
using System.Linq;
using Stats;
using UnityEngine;

namespace Quests
{
    public class ComplicatedKillingQuest : ScriptableObject,  IKillable
    {
        public event Action OnUpdateQuest;

        public void Kill(List<CombatTarget> combatTargets, int amount)
        {
            foreach (var combatTarget in combatTargets)
            {
                combatTarget.GetComponent<Health>().OnDied += () =>
                {
                    combatTargets.Remove(combatTarget);
                    amount--;
                    
                    if (!combatTargets.Any())
                    {
                        OnUpdateQuest?.Invoke();
                    }
                };
            }
        }
    }
}