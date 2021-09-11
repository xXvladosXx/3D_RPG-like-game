using Extensions;
using Projectile;
using Resistance;
using Stats;
using UnityEngine;

namespace Scriptable.Arrows
{
    [CreateAssetMenu(fileName = "Arrow", menuName = "ScriptableObjects/Arrow", order = 1)]
    public class ArrowScriptable : ScriptableObject
    {
        [SerializeField] private GameObject _projectile;
        [SerializeField] private float _speed;
        [SerializeField] private float _damage = 3f;
        [SerializeField] private DamageType _damageType = DamageType.Physical;
        
        private GameObject _attacker;
        private GameObject _target;
        private GameObject _projectileInstance;

        public void LoadProjectile(Transform target, Transform damager, SerializableDictionary<DamageType, float> damage)
        {
            if(target.GetComponent<Health>() == null) return;
            if(target.GetComponent<Health>().IsDead()) return;

            _projectileInstance = _projectile;

            _target = target.gameObject;
            _attacker = damager.gameObject;
            _projectileInstance = Instantiate(_projectile, 
                new Vector3(damager.position.x, damager.GetComponent<CapsuleCollider>().height/2, damager.position.z),
                Quaternion.identity);

            var damageClone = damage.ShallowClone();
            if (damageClone.TryGetValue(_damageType, out var currentDamage))
            {
                damageClone.Remove(_damageType);
                damageClone.Add(new SerializableDictionary<DamageType, float>.Pair(_damageType, currentDamage+_damage));
            }
        
            _projectileInstance.GetComponent<ProjectileAttack>().SetProjectileTarget(_target.GetComponent<Health>(), 
                damager.gameObject, _speed, damageClone);
        }
    }
}
