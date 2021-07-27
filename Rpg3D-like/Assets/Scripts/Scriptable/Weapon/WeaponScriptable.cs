using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Scriptable.Weapon;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class WeaponScriptable : ScriptableObject
{
    
    [SerializeField] private GameObject _prefabWeapon;
    [SerializeField] private AnimatorOverrideController _weaponOverrideController;
    [SerializeField] private Item.ItemType _itemType;
    [SerializeField] private Item.ItemLevel _itemLevel; 
    [SerializeField] private ArrowScriptable _arrow;
    [SerializeField] private Skill[] _weaponSkills;
    public Skill[] GetWeaponSkills => _weaponSkills;

    [SerializeField] private float _damage = 1f;
    public float GetDamage => _damage;
  
    [SerializeField] private float _attackRange = 1f;
    public float GetAttackRange => _attackRange;

    [SerializeField] private bool _isRightHanded = false;
    public bool GetWeaponHand => _isRightHanded;
    
    [SerializeField] private float _attackSpeed = 1f;
    public float GetAttackSpeed => _attackSpeed;

    private const string _weaponName = "Weapon";

    public void Spawn(Transform position, Animator animator)
    {
        if (_prefabWeapon != null)
        {
            GameObject weapon = Instantiate(_prefabWeapon, position);

            weapon.name = _weaponName;
        }

        var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

        if (_weaponOverrideController != null)
        {
            animator.runtimeAnimatorController = _weaponOverrideController;
        }else if (overrideController != null)
        {
            animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
        }
    }

    public void DestroyOldWeapon(Transform righthand, Transform lefthand)
    {
        Transform oldWeapon = righthand.Find("Weapon");

        if (oldWeapon == null)
        {
            oldWeapon = lefthand.Find("Weapon");
        }
        
        if(oldWeapon == null) return;
        
        Destroy(oldWeapon.gameObject);
    }
   
    public void SpawnProjectile(Transform target, Transform damager, float currentDamage)
    {
        _arrow.LoadProjectile(target, damager, currentDamage);
    }

    public ArrowScriptable GetProjectile()
    {
        return _arrow;
    }
    
    public Item.ItemType GetItemType()
    {
        return _itemType;
    }
    
    
}
