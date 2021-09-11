using Controller;
using Stats;
using UnityEngine;

namespace StateMachine.Enemy
{
    [CreateAssetMenu(fileName = "DistanceConfiguration", menuName = "Configuration/DistanceConfiguration")]
    public class StateDistanceConfiguration : ScriptableObject
    {
        public float ChaseDistance = 15f;
        public float DamageChaseDistance = 30f;
        
        public bool IsInRange(PlayerController playerController, Health health, float damage)
        {
            if (playerController == null || playerController.GetComponent<Health>().IsDead()) return false;
        
            return Vector3.Distance( health.transform.position, playerController.transform.position) < damage;
        }
    }
}