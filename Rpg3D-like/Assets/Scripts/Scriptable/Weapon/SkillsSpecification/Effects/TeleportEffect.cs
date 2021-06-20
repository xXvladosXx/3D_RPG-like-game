using System;
using System.Collections;
using UnityEngine;

namespace Scriptable.Weapon
{
    [CreateAssetMenu(fileName = "TeleportTargeting", menuName = "Abilities/TeleportPlayer", order = 0)]
    public class TeleportEffect : EffectStrategy
    {
        [SerializeField] private GameObject _spawnEffect;

        private bool _hasCompanion = false;
        private FriendlyAIController _spawnedCompanion;

        public override void Effect(SkillData skillData, Action finished)
        {
            Teleporting(skillData);

            finished();
        }

        private void Teleporting(SkillData skillData)
        {
            GameObject playerMesh = skillData.GetUser.gameObject.GetComponentInChildren<PlayerMesh>().gameObject;

            playerMesh.gameObject.SetActive(false);
            Instantiate(_spawnEffect, skillData.GetUser.transform.position, Quaternion.identity);

            skillData.GetUser.transform.position = skillData.GetMousePosition;
            skillData.GetUser.GetComponent<Movement>().StartMoveToAction(skillData.GetMousePosition, 1f);
            
            playerMesh.gameObject.SetActive(true);
            Instantiate(_spawnEffect, skillData.GetMousePosition, Quaternion.identity);
        }

        private IEnumerator WaitToTeleport()
        {
            yield return new WaitForSecondsRealtime(2f);
        }

    }
}