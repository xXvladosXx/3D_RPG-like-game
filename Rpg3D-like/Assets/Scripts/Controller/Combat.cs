using System;
using System.Collections;
using System.Collections.Generic;
using Interface;
using UnityEngine;

public class Combat : MonoBehaviour, IAction, IModifierStat
{
    [SerializeField] private float _attackRange = 3f;
    [SerializeField] private float _attackSpeed = 2f;
    [SerializeField] private Transform _rightHandTransform;
    [SerializeField] private Transform _leftHandTransform;

    private Animator _animator;
    private float _attackCooldown = 0;
    private Transform _target;
    private ActionScheduler _actionScheduler;
    private FindStat _findStat;
    [SerializeField] private WeaponScriptable _weapon;

    [SerializeField] private float _defaultDamage = 1f;
    [SerializeField] private float _currentDamage = 1f;
    
    private Health _health;
    private bool _isWeaponEquipped = false;

    
    public Transform GetTarget => _target;

    
    private void Awake()
    {
        _health = GetComponent<Health>();
        _animator = GetComponent<Animator>();
        _actionScheduler = GetComponent<ActionScheduler>();
        _findStat = GetComponent<FindStat>();
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
    public void EquipWeapon(WeaponScriptable weapon,bool isRightHanded, WeaponPickUp weaponPickUp, float damage, float range, float attackSpeed)
    {
        if(weapon == null) return;
        
        GameObject oldWeapon = GameObject.Find("Weapon");

        if (_isWeaponEquipped)
        {
            Destroy(oldWeapon);
        }

        weaponPickUp.OnWeaponPicked += CheckingForTwoHandedWeapon;
        
        _weapon = weapon;
        _attackRange = range;
        _attackSpeed = attackSpeed;
        
        if (gameObject.TryGetComponent(out PlayerController playerController))
        {
            GetComponent<PlayerController>().SetWeapon(weapon);
        }
        
        Animator animator = GetComponent<Animator>();

        weapon.Spawn(isRightHanded ? _rightHandTransform : _leftHandTransform, animator);

        _isWeaponEquipped = true;
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
    
    public IEnumerable<float> GetStatModifier(StatsEnum stat)
    {
        if (stat == StatsEnum.Damage)
        {
            yield return _weapon.GetDamage;
        }
    }
    
    void Hit()
    {
        if(_target == null) return;

        if(_target.GetComponent<Health>() == null) return;  
     
        _currentDamage = _findStat.GetStat(StatsEnum.Damage);

        _target.GetComponent<Health>().TakeDamage(_currentDamage, gameObject);
    }

    void Shoot()
    {
        if(_target == null) return;
        if(_weapon.GetProjectile() == null) return;
        
        _currentDamage = _findStat.GetStat(StatsEnum.Damage);

        print(_currentDamage);
        transform.LookAt(_target);
        _weapon.SpawnProjectile(_target, gameObject.transform, _currentDamage);
    }
    
    void FootR()
    {
        
    }
    
    void FootL()
    {
        
    }

    
}
