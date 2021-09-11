using System;
using UnityEngine;

namespace Scriptable.Weapon
{

    [CreateAssetMenu(fileName = "Projectile Spawning", menuName = "Abilities/SpellCastingProjectile", order = 0)]
    public class SpawnProjectileEffect : EffectStrategy
    {
        [SerializeField] private ProjectileAttack projectileToSpawn;
        [SerializeField] private int _amountToSpawn = 1;
        [SerializeField] private float _damage;
        [SerializeField] private float _damageModifier;
        [SerializeField] private float _speed;
        [SerializeField] private bool _isPointTarget = true;
        [SerializeField] private float _radius = 5f;
        [SerializeField] private bool _selfCasted = false;
        public override void Effect(SkillData skillData, Action finished)
        {
            if (_isPointTarget)
            {
                SingleProjectileAttack(skillData);
            }
            else
            {
                RadiusProjectileAttack(skillData);
            }

            finished();
        }

        private void SingleProjectileAttack(SkillData skillData)
        {
            skillData.GetUser.transform.LookAt(skillData.GetMousePosition);

            Vector3 startPoint = skillData.GetUser.transform.position;
            
            float angleStep = 360f / _amountToSpawn;
            float angle = 0f;

            for (int i = 0; i <= _amountToSpawn - 1; i++) {
                ProjectileAttack projectileAttack = skillData.GetUser.GetComponentInChildren<ObjectPooler>().GetPooledObject().GetComponent<ProjectileAttack>();
                Debug.Log(FindObjectOfType<ObjectPooler>());
                if(projectileAttack == null) return;

                float projectileDirXposition = startPoint.x + Mathf.Sin ((angle * Mathf.PI) / 180) * _radius;
                float projectileDirZposition = startPoint.z + Mathf.Cos ((angle * Mathf.PI) / 180) * _radius;

                Vector3 projectileVector = new Vector3(projectileDirXposition, 0.5f, projectileDirZposition);
                Vector3 projectileMoveDirection = (projectileVector - startPoint).normalized * _speed;

                projectileAttack.transform.position = new Vector3(skillData.GetUser.transform.position.x,
                    skillData.GetUser.transform.position.y + 0.5f, skillData.GetUser.transform.position.z);
                
                projectileAttack.transform.rotation = Quaternion.identity;
                Debug.Log(projectileAttack);
                projectileAttack.gameObject.SetActive(true);
                
                projectileAttack.GetComponent<Rigidbody> ().velocity = 
                    new Vector3 (projectileMoveDirection.x, 0.5f, projectileMoveDirection.z);

                projectileAttack.SetProjectileTarget(
                    new Vector3(projectileMoveDirection.x, 0.5f, projectileMoveDirection.z)
                    , skillData.GetUser, _damage+skillData.GetUser.GetComponent<Combat>().GetCurrentDamage*_damageModifier, _speed);
                
                angle += angleStep;
            }
            
        }

        private void RadiusProjectileAttack(SkillData skillData)
        {
            foreach (var target in skillData.GetTargets)
            {
                Health health = target.GetComponent<Health>();
                if (!health) continue;
                
                ProjectileAttack projectileSpawn = Instantiate(projectileToSpawn, new Vector3(skillData.GetUser.transform.position.x, 
                        skillData.GetUser.GetComponent<CapsuleCollider>().height/2, skillData.GetUser.transform.position.z),
                    Quaternion.identity);
                projectileSpawn.SetProjectileTarget(health, skillData.GetUser, _damage + skillData.GetUser.GetComponent<Combat>().GetCurrentDamage*_damageModifier, _speed);
            }
        }
    }
}