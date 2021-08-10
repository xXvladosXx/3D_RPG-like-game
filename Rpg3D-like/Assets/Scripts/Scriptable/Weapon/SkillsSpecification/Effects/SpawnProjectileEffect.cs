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
            
            for (int i = 0; i < _amountToSpawn; i++)
            {
                ProjectileAttack projectileAttack = ObjectPooler.CurrentObjectPooler.GetPooledObject();
                if(projectileAttack == null) return;

                projectileAttack.transform.position = new Vector3(skillData.GetUser.transform.position.x,
                    skillData.GetUser.transform.position.y + 0.5f, skillData.GetUser.transform.position.z);
            
                projectileAttack.transform.rotation = Quaternion.identity;
            
                projectileAttack.gameObject.SetActive(true);

                if (i % 2 == 0)
                {
                    projectileAttack.SetProjectileTarget(
                        new Vector3(skillData.GetMousePosition.x + i * 1, skillData.GetMousePosition.y,
                            skillData.GetMousePosition.z + i * 1)
                        , skillData.GetUser, _damage+skillData.GetUser.GetComponent<Combat>().GetCurrentDamage*_damageModifier, _speed);
                    
                    Debug.Log(_damage+skillData.GetUser.GetComponent<Combat>().GetCurrentDamage*_damageModifier);
                }
                else
                {
                    projectileAttack.SetProjectileTarget(
                        new Vector3(skillData.GetMousePosition.x + i * -1, skillData.GetMousePosition.y,
                            skillData.GetMousePosition.z + i * -1)
                        , skillData.GetUser, _damage+skillData.GetUser.GetComponent<Combat>().GetCurrentDamage*_damageModifier, _speed);
                }
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