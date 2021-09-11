using Controller;
using Resistance;
using Stats;
using UnityEngine;
using UnityEngine.AI;

namespace Scriptable.Weapon.SkillsSpecification.Effects
{
   public class ThrowSkill : MonoBehaviour
   {
      [SerializeField] private string _target;
      [SerializeField] private float _force;
      [SerializeField] private float _damage;
      [SerializeField] private DamageType _damageType = DamageType.Physical;
      private void Start()
      {
         Destroy(gameObject, 2);
      }

      private void FixedUpdate()
      {
         Make();
      }

      private void Make()
      {
         float radius =0.5f;
         Collider[] objectsInRange = Physics.OverlapSphere(transform.position, radius);

         foreach (Collider col in objectsInRange)
         {
            Rigidbody enemy = col.GetComponent<Rigidbody>();

            if (enemy == null || !enemy.CompareTag(_target)) continue;
            enemy.GetComponent<NavMeshAgent>().enabled = false;
            enemy.AddForce(Vector3.up * _force, ForceMode.Impulse);
         }
      }

      private void OnTriggerEnter(Collider other)
      {
         if (!other.CompareTag(_target)) return;
      
         if (!other.GetComponent<Health>().IsDead())
            other.GetComponent<Health>().TakeDamage(_damage, FindObjectOfType<PlayerController>().gameObject, _damageType);
      }
   }
}