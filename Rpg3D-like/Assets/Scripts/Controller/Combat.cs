using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour, IAction
{
    [SerializeField] private float _attackRange = 3f;
    [SerializeField] private float _attackSpeed = 2f;
    [SerializeField] private Transform _rightHandTransform;
    [SerializeField] private Transform _leftHandTransform;

    private Animator _animator;
    private float _attackCooldown = 0;
    private Transform _target;
    private ActionScheduler _actionScheduler;
    public Transform GetTarget => _target;
    
    private WeaponScriptable _weapon;
    private float _damage = 1f;
    private Health _health;
    private bool isWeaponEquipped = false;
    
    private void Awake()
    {
        _health = GetComponent<Health>();
        _animator = GetComponent<Animator>();
        _actionScheduler = GetComponent<ActionScheduler>();
    }

    void Update()
    {
        if(_target == null) return;
        if(_health.IsDead()) return;

        bool isInRange = GetDistance();
        if (!isInRange)
        {
            GetComponent<Movement>().MoveTo(_target.position, 1f);
        }
        else{
            GetComponent<Movement>().Cancel();
            
            if(_attackCooldown <= 0)
                TriggerStartAttack();
        }

        if (_attackCooldown > 0)
        {
            _attackCooldown -= Time.deltaTime;
        }
    }
    

    private bool GetDistance()
    {
        return Vector3.Distance(gameObject.transform.position, _target.transform.position) < _attackRange;
    }

    public bool CanAttack(GameObject target)
    {
        if (target.GetComponent<Health>() == null) return false;
        if (target.GetComponent<Health>().IsDead())
        {
            Cancel();

            return false;
        }

        Health targetHealth = target.GetComponent<Health>();

        return targetHealth != null;
    }
    
    public void Attack(Transform target)
    {
        _actionScheduler.StartAction(this);

        _target = target.transform;
        transform.LookAt(_target);
    }

    private void TriggerStartAttack()
    { 
        if(_target.GetComponent<Health>() ==null) return;
        if(_target.GetComponent<Health>().IsDead()) return;
        
        _attackCooldown = 1;

        _animator.ResetTrigger("stopAttack");
        
        
        _animator.SetTrigger("attack");
    }

    private void TriggerInterruptAttack()
    {
        _animator.ResetTrigger("attack");
        _animator.SetTrigger("stopAttack");
    }
    public void EquipWeapon(WeaponScriptable weapon,bool isRightHanded, WeaponPickUp weaponPickUp, float damage, float range)
    {
        if(weapon == null) return;
        
        GameObject oldWeapon = GameObject.Find("Weapon");

        if (isWeaponEquipped)
        {
            Destroy(oldWeapon);
        }

        weaponPickUp.OnWeaponPicked += CheckingForTwoHandedWeapon;
        
        _weapon = weapon;
        _damage = damage;
        _attackRange = range;

        if (gameObject.TryGetComponent(out PlayerController playerController))
        {
            GetComponent<PlayerController>().SetWeapon(weapon);
        }
        
        Animator animator = GetComponent<Animator>();

        weapon.Spawn(isRightHanded ? _rightHandTransform : _leftHandTransform, animator);

        isWeaponEquipped = true;
    }

    private void CheckingForTwoHandedWeapon()
    {
        GameObject[] oldWeapons = GameObject.FindGameObjectsWithTag("Weapon");
        
        if(oldWeapons.Length > 2)
            Destroy(oldWeapons[1]);
    }

    public void Cancel()
    {
        TriggerInterruptAttack();
        
        _target = null;
    }
    
    void Hit()
    {
        if(_target == null) return;

        if(_target.GetComponent<Health>() == null) return;  
        
        _target.GetComponent<Health>().TakeDamage(_damage, gameObject);
    }

    void Shoot()
    {
        
        if(_target == null) return;
        
        if(_weapon.GetProjectile() == null) return;
        
        transform.LookAt(_target);
        GetComponent<ProjectileSpawn>().LoadProjectile(_weapon.GetProjectile(), _target, gameObject.transform, 2f, _damage);
    }
    
    void FootR()
    {
        
    }
    
    void FootL()
    {
        
    }
}
