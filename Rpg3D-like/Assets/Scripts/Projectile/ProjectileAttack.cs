using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    [SerializeField] private GameObject _projectileTigger;
    
    private GameObject _target;
    private GameObject _damager;
    private float _damage;
    private float _speed;
    private Vector3 _targetPoint;
    private Vector3 normalizeDirection;

    private void Start()
    {
        normalizeDirection = (_targetPoint - transform.position).normalized;
    }

    private void Update()
    {
        GetTargetToAttack();
    }

    private void GetTargetToAttack()
    {
        if (_target == null)
        {
            gameObject.transform.position += normalizeDirection * (_speed*10) * Time.deltaTime;
            Destroy(gameObject, 3f);
        }
        else
        {
            transform.LookAt(_target.transform);

            if (!_target.GetComponent<Health>().IsDead())
            {
                gameObject.transform.position =
                    Vector3.MoveTowards(gameObject.transform.position,
                        new Vector3(_target.transform.position.x, _target.GetComponent<CapsuleCollider>().height / 2,
                            _target.transform.position.z),
                        _speed / 10);
            }
        }
    }

    public void SetProjectileTarget(GameObject target, GameObject damager, float damage, float speed)
    {
        SetProjectileTarget(damager, damage, speed, target);
    }

    public void SetProjectileTarget(Vector3 targetPoint, GameObject damager, float damage, float speed)
    {
        SetProjectileTarget(damager, damage, speed,null, targetPoint);
    }
    
    public void SetProjectileTarget(GameObject damager, float damage, float speed, GameObject target = null, Vector3 targetPoint=default)
    {   
        if(target !=null)
            Debug.Log("Target aquired" + target + _speed);
        
        _damager = damager;
        _target = target;
        _damage = damage;
        _speed = speed;
        _targetPoint = targetPoint;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<Health>().IsDead() && other.gameObject != _damager)
        {
            if (_projectileTigger != null)
                Instantiate(_projectileTigger, gameObject.transform.position, Quaternion.identity);
                
            print("entered");
            Destroy(gameObject);
            other.GetComponent<Health>().TakeDamage(_damage, _damager);
        }
        
    }
}
