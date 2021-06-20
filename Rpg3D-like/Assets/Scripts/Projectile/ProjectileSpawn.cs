using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawn : MonoBehaviour
{
    private GameObject _projectile;
    private float _speed;
    private GameObject _attacker;
    private GameObject _target;

    [SerializeField] private Transform _spawnPosition;
    public void LoadProjectile(GameObject projectile, Transform target, Transform attacker, float speed, float damage)
    {
        if(target.GetComponent<Health>() == null) return;
        if(target.GetComponent<Health>().IsDead()) return;
            
        _target = target.gameObject;
        _speed = speed;
        _attacker = attacker.gameObject;
        _projectile = Instantiate(projectile, new Vector3(attacker.position.x, attacker.GetComponent<CapsuleCollider>().height/2, attacker.position.z) , Quaternion.identity);
        
        _projectile.GetComponent<ProjectileAttack>().SetProjectileTarget(_target, gameObject, damage, speed);
    }

   
}
