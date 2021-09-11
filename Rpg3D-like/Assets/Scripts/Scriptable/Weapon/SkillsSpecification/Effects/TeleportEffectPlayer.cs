using System;
using System.Collections;
using Controller;
using UnityEngine;

namespace Scriptable.Weapon.SkillsSpecification.Effects
{
    [CreateAssetMenu(fileName = "TeleportTargeting", menuName = "Abilities/Core/TeleportEffectPlayer", order = 0)]
    public class TeleportEffectPlayer : TeleportEffect
    {
        public override void Effect(SkillData skillData, Action finished)
        {
            Teleporting(skillData, skillData.GetMousePosition);

            finished();
        }

        public override void SetData(DataCollector dataCollector)
        {
            
        }

        protected override void Teleporting(SkillData skillData, Vector3 position)
        {
            GameObject playerMesh = skillData.GetUser.gameObject.GetComponentInChildren<PlayerMesh>().gameObject;

            playerMesh.gameObject.SetActive(false);
            Instantiate(_spawnEffect, skillData.GetUser.transform.position, Quaternion.identity);

            skillData.GetUser.transform.position = position;
            skillData.GetUser.GetComponent<Movement>().StartMoveToAction(position, 1f);
            
            playerMesh.gameObject.SetActive(true);
            Instantiate(_spawnEffect, position, Quaternion.identity);
        }
    }
}