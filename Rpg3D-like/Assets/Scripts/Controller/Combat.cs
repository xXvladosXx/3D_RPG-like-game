using System;
using System.Collections;
using System.Collections.Generic;
using Interface;
using Saving;
using UnityEngine;

public class Combat : MonoBehaviour, IAction, IModifierStat, ISaveable
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
    
    [SerializeField] private string _defaultWeaponName = "Sword";
    
    [SerializeField] private WeaponScriptable _defaultWeapon = null;
    [SerializeField] private WeaponScriptable _currentWeapon;

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
        
        WeaponScriptable weaponScriptable = Resources.Load<WeaponScriptable>(_defaultWeaponName);
        _defaultWeapon = weaponScriptable;
    }

    private void Start()
    {
        if(_currentWeapon == null)
            EquipWeapon(_defaultWeapon);
    }

    void Update()
    {
        if(_target == null) return;
        if (_target.GetComponent<Health>().IsDead())
        {
            _target = FindNewTarget();
            if(_target == null) return;
        }

        bool isInRange = GetDistance();
        if (!isInRange)
        {
            GetComponent<Movement>().MoveTo(_target.position , 1f);
        }
        else
        {
            GetComponent<Movement>().Cancel();
            AttackBehaviour();
        }

        if (_attackCooldown > 0)
        {
            _attackCooldown -= Time.deltaTime;
        }
    }

    public Transform FindNewTarget()
    {
        CombatTarget[] targets = FindObjectsOfType<CombatTarget>();
        float minDistance = 3f;

        foreach (CombatTarget target in targets)
        {
            if(target.GetComponent<Health>().IsDead()) continue;
            
            float distanceToTarget = Vector3.Distance(gameObject.transform.position, target.transform.position);

            if (minDistance > distanceToTarget)
            {
                minDistance = distanceToTarget;
                return target.transform;
            }
        }

        return null;
    }

    private void AttackBehaviour()
    {
        if (_attackCooldown <= 0)
            TriggerStartAttack();
    }


    public bool GetDistance()
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
        if(target.GetComponent<Health>().IsDead()) return;
        
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
    public void EquipWeapon(WeaponScriptable weapon)
    {
        if(weapon == null) return;
        
        _currentWeapon = weapon;
        _attackRange = weapon.GetAttackRange;
        _attackSpeed = weapon.GetAttackSpeed;
        
        if (gameObject.TryGetComponent(out PlayerController playerController))
        {
            GetComponent<PlayerController>().InventoryPlacerWeapon(weapon);
            GetComponent<PlayerSkills>().SetPlayerSkills(weapon.GetWeaponSkills);
        }
        
        Animator animator = GetComponent<Animator>();

        weapon.DestroyOldWeapon(_rightHandTransform, _leftHandTransform);
        weapon.Spawn(weapon.GetWeaponHand ? _rightHandTransform : _leftHandTransform, animator);

            
        _isWeaponEquipped = true;
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
            yield return _currentWeapon.GetDamage;
        }
    }
    
    void Hit()
    {
        if(_target == null) return;

        if(_target.GetComponent<Health>() == null) return;  
     
        _currentDamage = _findStat.GetStat(StatsEnum.Damage);
        transform.LookAt(_target);
        _target.GetComponent<Health>().TakeDamage(_currentDamage, gameObject);
    }

    void Shoot()
    {
        if(_target == null) return;
        if(_currentWeapon.GetProjectile() == null) return;
        
        _currentDamage = _findStat.GetStat(StatsEnum.Damage);
        transform.LookAt(_target);
        _currentWeapon.SpawnProjectile(_target, gameObject.transform, _currentDamage);
    }
    
    void FootR()
    {
        
    }
    
    void FootL()
    {
        
    }


    public object CaptureState()
    {
        if (_currentWeapon == null)
        {
            return _defaultWeapon.name;
        }

        return _currentWeapon.name;
    }

    public void RestoreState(object state)
    {
        string weaponName = (string) state;
        WeaponScriptable weapon = Resources.Load<WeaponScriptable>(weaponName);
        EquipWeapon(weapon);
    }
}
