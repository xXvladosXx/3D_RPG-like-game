using System;
using Controller;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Scriptable.Weapon.SkillsSpecification.Effects
{
    [CreateAssetMenu(fileName = "TeleportTargeting", menuName = "Abilities/Core/TeleportEffectEntity", order = 0)]
    public class TeleportEffectEntity : TeleportEffect
    {
        [SerializeField] private float _radius;
        [SerializeField] private float _minDistanceToAttack;
        public override void Effect(SkillData skillData, Action finished)
        {
            var playerPosition = FindObjectOfType<PlayerController>().transform.position;
            var newPosition = skillData.GetUser.transform.position;

            Vector3 centerPosition = playerPosition; 
            float distance = Vector3.Distance(newPosition, centerPosition); 

            if (distance > _radius && distance < _minDistanceToAttack)
            {
                Vector3 fromOriginToObject = newPosition - centerPosition; 
                fromOriginToObject *= _radius / distance; 
                newPosition = centerPosition + fromOriginToObject;
            }
            else
            {
                newPosition = RandomNavmeshLocation(skillData);
            }
            
            Teleporting(skillData, newPosition);
            
            finished();
        }

        public override void SetData(DataCollector dataCollector)
        {
            dataCollector.AddDataFromNewLine("Radius " + _radius);
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
        
        private Vector3 RandomNavmeshLocation(SkillData skillData) {
            Vector3 randomDirection = Random.insideUnitSphere * _radius;
            randomDirection += skillData.GetUser.transform.position;
            NavMeshHit hit;
            Vector3 finalPosition = Vector3.zero;
            if (NavMesh.SamplePosition(randomDirection, out hit, _radius, 1)) {
                finalPosition = hit.position;            
            }
            return finalPosition;
        }
    }
}