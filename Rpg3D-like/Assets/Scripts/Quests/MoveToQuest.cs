using System;
using Stats;
using TMPro;
using UnityEngine;

namespace Quests
{
    [CreateAssetMenu(fileName = "MovingToQuest", menuName = "Quests/MovingQuest", order = 0)]
    public class MoveToQuest : InitializationQuest
    {

        private GameObject _questGameObject;
        
        public override void InitQuest(Action completed)
        {
            GameObject player = GameObject.FindWithTag("Player");
            
            AreaActivator questPlaneLocation = _questGameObject.GetComponent<AreaActivator>();
            
            questPlaneLocation.ActivateArea();

            questPlaneLocation.OnLocationEntered += () =>
            {
                player.GetComponent<LevelUp>().ExperienceReward(Experience);
                questPlaneLocation.DestroyArea();
                completed();
            };
        }

        public override GameObject GetAim()
        {
            return _questGameObject;
        }
    }
}