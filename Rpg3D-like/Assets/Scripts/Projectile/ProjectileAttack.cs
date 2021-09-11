using Resistance;
using Stats;
using UnityEngine;

namespace Projectile
{
    public class ProjectileAttack : MonoBehaviour
    {
        [SerializeField] private GameObject _projectileTrigger;
        [SerializeField] private bool _autoTargeting;
    
        private Health _target;
        private GameObject _damager;
        private float _speed;
        private Vector3 _targetPoint;
        private SerializableDictionary<DamageType, float> _damagePairs;

        private void Start()
        {
            transform.LookAt(GetAim());
        }

        private void OnEnable()
        {
            transform.LookAt(GetAim());

            Invoke("Disable", 2f);
        }

        private void Update()
        {
            if (_autoTargeting)
            {
                transform.LookAt(GetAim());
            }

            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

        private void GetTargetToAttack()
        {
            GetAim();

            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }

        private Vector3 GetAim()
        {
            if (_target == null) return _targetPoint;

            CapsuleCollider capsuleCollider = _target.GetComponent<CapsuleCollider>();
            if (capsuleCollider == null)
            {
                return _target.transform.position;
            }

            return _target.transform.position + Vector3.up * capsuleCollider.height / 2;
        }

        public void SetProjectileTarget(Health target, GameObject damager, float speed,
            SerializableDictionary<DamageType, float> serializableDictionary)
        {
            SetProjectileTarget(damager, speed, serializableDictionary, target);
        }
        public void SetProjectileTarget(Vector3 targetPoint, GameObject damager, float damage, float speed,
            DamageType damageType = DamageType.Physical)
        {
            print(damage);

            SetProjectileTarget(damager, speed, new SerializableDictionary<DamageType, float>(){new SerializableDictionary<DamageType, float>.Pair(damageType, damage)}, null, targetPoint);
        }
        
        public void SetProjectileTarget(Vector3 targetPoint, GameObject damager, float speed,
            SerializableDictionary<DamageType, float> damagePairs)
        {
            SetProjectileTarget(damager, speed, damagePairs, null, targetPoint);
        }


        private void SetProjectileTarget(GameObject damager, float speed, SerializableDictionary<DamageType, float> damagePairs,
            Health target = null, Vector3 targetPoint = default)
        {
            _damager = damager;
            _target = target;
            _speed = speed;
            _damagePairs = damagePairs;
            _targetPoint = targetPoint;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() == null) return;

            if (!other.gameObject.GetComponent<Health>().IsDead() && other.gameObject != _damager)
            {
                if (_projectileTrigger != null)
                {
                    GameObject projectilePostEffect = Instantiate(_projectileTrigger, gameObject.transform.position,
                        other.transform.rotation);
                    Destroy(projectilePostEffect, 0.5f);
                }
            
                other.GetComponent<Health>().TakeDamage(_damagePairs, _damager);
                Disable();
            }
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        
            if (gameObject.GetComponentInParent<ObjectPooler.ObjectPooler>()==null)
            {
                Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
            transform.LookAt(GetAim());

            CancelInvoke();
        }
    }
}