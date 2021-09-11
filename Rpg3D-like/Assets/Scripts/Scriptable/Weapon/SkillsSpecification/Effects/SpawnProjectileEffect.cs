using System;
using System.Collections.Generic;
using Controller;
using Extensions;
using Projectile;
using Resistance;
using Scriptable.Weapon.SkillsSpecification.Strategies;
using Stats;
using UnityEngine;

namespace Scriptable.Weapon.SkillsSpecification.Effects
{
    [CreateAssetMenu(fileName = "Projectile Spawning", menuName = "Abilities/Core/ProjectileEffect", order = 0)]
    public class SpawnProjectileEffect : EffectStrategy
    {
        [SerializeField] private ProjectileAttack projectileToSpawn;
        [SerializeField] private DamageType _damageType;
        [SerializeField] private int _amountToSpawn = 1;
        [SerializeField] private float _damage;
        [SerializeField] private float _damageModifier;
        [SerializeField] private float _speed;
        [SerializeField] private bool _isPointTarget = true;
        [SerializeField] private float _radius = 5f;
        [SerializeField] private bool _radiusSpawn = false;

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

        public override void SetData(DataCollector dataCollector)
        {
            dataCollector.AddDataFromNewLine("Amount " + _amountToSpawn);
            dataCollector.AddDataFromNewLine("Damage " + _damage +" * " + _damageModifier);
        }

        private void SingleProjectileAttack(SkillData skillData)
        {
            skillData.GetUser.transform.LookAt(skillData.GetMousePosition);

            Vector3 startPoint = skillData.GetUser.transform.position;

            float angleStep = 360f / _amountToSpawn;
            float angle = 0f;

            if (_radiusSpawn)
            {
                for (int i = 0; i <= _amountToSpawn - 1; i++)
                {
                    ProjectileAttack projectileAttack = skillData.GetUser.GetComponentInChildren<ObjectPooler.ObjectPooler>()
                        .GetPooledObject().GetComponent<ProjectileAttack>();
                    if (projectileAttack == null) return;

                    float projectileDirXposition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * _radius;
                    float projectileDirZposition = startPoint.z + Mathf.Cos((angle * Mathf.PI) / 180) * _radius;

                    Vector3 projectileVector = new Vector3(projectileDirXposition, 0.5f, projectileDirZposition);
                    Vector3 projectileMoveDirection = (projectileVector - startPoint).normalized * _speed;

                    var attackTransform = projectileAttack.transform;
                    var playerPosition = skillData.GetUser.transform.position;
                    attackTransform.position = new Vector3(playerPosition.x,
                        playerPosition.y + 0.5f, playerPosition.z);

                    attackTransform.rotation = Quaternion.identity;
                    projectileAttack.gameObject.SetActive(true);

                    projectileAttack.GetComponent<Rigidbody>().velocity =
                        new Vector3(projectileMoveDirection.x, 0.5f, projectileMoveDirection.z);

                    Debug.Log(skillData.GetUser.GetComponent<Equipment>().GetCurrentWeapon._damage);


                    var c = skillData.GetUser.GetComponent<Equipment>().GetCurrentWeapon._damage.ShallowClone();
                    if (c.TryGetValue(_damageType, out var d))
                    {
                        c.Remove(_damageType);
                        c.Add(new SerializableDictionary<DamageType, float>.Pair(_damageType, d + _damage * _damageModifier));
                    }
                    
                    projectileAttack.SetProjectileTarget(
                        new Vector3(projectileMoveDirection.x, 0.5f, projectileMoveDirection.z)
                        , skillData.GetUser,
                        _speed, c);

                    angle += angleStep;
                }
            }
            else
            {
                for (int i = 0; i < _amountToSpawn; i++)
                {
                    var playerPosition = skillData.GetUser.transform.position;
                    ProjectileAttack projectile = Instantiate(projectileToSpawn,
                        new Vector3(playerPosition.x, playerPosition.y + 0.5f, playerPosition.z), Quaternion.identity);

                    projectile.SetProjectileTarget(new Vector3(skillData.GetMousePosition.x,
                        skillData.GetMousePosition.y + 0.5f,
                        skillData.GetMousePosition.z), skillData.GetUser, _damage, _speed);
                }
            }
        }

        private void RadiusProjectileAttack(SkillData skillData)
        {
            foreach (var target in skillData.GetTargets)
            {
                Health health = target.GetComponent<Health>();
                if (!health) continue;

                ProjectileAttack projectileSpawn = Instantiate(projectileToSpawn, new Vector3(
                        skillData.GetUser.transform.position.x,
                        skillData.GetUser.GetComponent<CapsuleCollider>().height / 2,
                        skillData.GetUser.transform.position.z),
                    Quaternion.identity);
                if (skillData.GetUser.GetComponent<Equipment>().GetCurrentWeapon._damage
                    .TryGetValue(DamageType.Physical, out var d) == true)
                {
                    projectileSpawn.SetProjectileTarget(skillData.GetUser.transform.position, skillData.GetUser,
                        _damage + d * _damageModifier, _speed);
                }
            }
        }
    }
}