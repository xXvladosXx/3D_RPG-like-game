using System.Collections;
using System.Collections.Generic;
using Interface;
using UnityEngine;

[CreateAssetMenu(fileName = "Arrow", menuName = "ScriptableObjects/Arrow", order = 1)]
public class ArrowScriptable : ScriptableObject
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage = 3f;
    
    private GameObject _attacker;
    private GameObject _target;
    private GameObject _projectileInstance;

    public void LoadProjectile(Transform target, Transform damager, float currentDamage)
    {
        if(target.GetComponent<Health>() == null) return;
        if(target.GetComponent<Health>().IsDead()) return;

        _projectileInstance = _projectile;

        _target = target.gameObject;
        _attacker = damager.gameObject;
        _projectileInstance = Instantiate(_projectile, new Vector3(damager.position.x, damager.GetComponent<CapsuleCollider>().height/2, damager.position.z) , Quaternion.identity);
        
        _projectileInstance.GetComponent<ProjectileAttack>().SetProjectileTarget(_target.GetComponent<Health>(), damager.gameObject, _damage + currentDamage, _speed);
    }
}
