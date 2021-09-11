using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    [SerializeField] private GameObject _projectileTrigger;
    
    private Health _target;
    private GameObject _damager;
    private float _damage;
    private float _speed;
    private Vector3 _targetPoint;
    
    private void OnEnable()
    {
        transform.LookAt(GetAim());
        Invoke("Disable", 2f);
    }

    private void Update()
    {
        transform.LookAt(GetAim());
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }   

    private void GetTargetToAttack()
    {
        GetAim();

        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private Vector3 GetAim()
    {
        if(_target == null) return _targetPoint;
        
        CapsuleCollider capsuleCollider = _target.GetComponent<CapsuleCollider>();
        if (capsuleCollider == null)
        {
            return _target.transform.position;
        }

        return _target.transform.position + Vector3.up * capsuleCollider.height / 2;
    }

    public void SetProjectileTarget(Health target, GameObject damager, float damage, float speed)
    {
        SetProjectileTarget(damager, damage, speed, target);
    }

    public void SetProjectileTarget(Vector3 targetPoint, GameObject damager, float damage, float speed)
    {
        SetProjectileTarget(damager, damage, speed,null, targetPoint);

    }
    
    private void SetProjectileTarget(GameObject damager, float damage, float speed, Health target = null, Vector3 targetPoint=default)
    {   
        _damager = damager;
        _target = target;
        _damage = damage;
        _speed = speed;
        _targetPoint = targetPoint;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Health>() == null) return;
        
        if (!other.gameObject.GetComponent<Health>().IsDead() && other.gameObject != _damager)
        {
            if (_projectileTrigger != null)
            {
                GameObject projectilePostEffect = Instantiate(_projectileTrigger, gameObject.transform.position, other.transform.rotation);
                Destroy(projectilePostEffect, 0.5f);
            }

            Disable();
            other.GetComponent<Health>().TakeDamage(_damage, _damager);
        }
        
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        transform.LookAt(GetAim());

        CancelInvoke();
    }
}
