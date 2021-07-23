using System;
using UnityEngine;

namespace Scriptable.Weapon
{

    [CreateAssetMenu(fileName = "Projectile Spawning", menuName = "Abilities/SpellCastingProjectile", order = 0)]
    public class SpawnProjectileEffect : EffectStrategy
    {
        [SerializeField] private ProjectileAttack projectileSpawnToSpawn;
        [SerializeField] private float _damage;
        [SerializeField] private float _speed;
        [SerializeField] private bool _isPointTarget = true;
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
            ProjectileAttack projectileSpawn = Instantiate(projectileSpawnToSpawn, new Vector3(skillData.GetUser.transform.position.x, 
                    skillData.GetUser.transform.position.y+0.5f, skillData.GetUser.transform.position.z),
                Quaternion.identity);
            
            projectileSpawn.SetProjectileTarget(skillData.GetMousePosition, skillData.GetUser, _damage, _speed);
        }

        private void RadiusProjectileAttack(SkillData skillData)
        {
            foreach (var VARIABLE in skillData.GetTargets)
            {
                Health health = VARIABLE.GetComponent<Health>();
                if (health)
                {
                    ProjectileAttack projectileSpawn = Instantiate(projectileSpawnToSpawn, new Vector3(skillData.GetUser.transform.position.x, 
                            skillData.GetUser.GetComponent<CapsuleCollider>().height/2, skillData.GetUser.transform.position.z),
                        Quaternion.identity);
                    projectileSpawn.SetProjectileTarget(health.gameObject, skillData.GetUser, _damage, _speed);
                }
            }
        }
    }
}