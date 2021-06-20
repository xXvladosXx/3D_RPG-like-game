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
    [SerializeField] private ProgressionWeaponClass[] _weaponClasses;

    private Dictionary<WeaponEnum, Dictionary<WeaponStatsEnum, int[]>> _lookThroughWeapon;
    
    [SerializeField] private GameObject _prefabWeapon;
    [SerializeField] private AnimatorOverrideController _weaponOverrideController;
    
    

    [SerializeField] private GameObject _projectile;
    [SerializeField] private Sprite _weaponSprite;
    [SerializeField] private Item.ItemType _itemType;
    [SerializeField] private ParticleSystem[] _raritiesEffects;
    
    [SerializeField] private Skill[] _weaponSkills;
    public Skill[] GetWeaponSkills => _weaponSkills;
    [SerializeField] private float _damage = 1f;
    public float GetDamage => _damage;
    [SerializeField] private float _attackRange = 1f;
    public float GetAttackRange => _attackRange;
    [SerializeField] private float _attackSpeed = 1f;
    public float GetAttackSpeed => _attackSpeed;

    private const string _weaponName = "Weapon";

    public void Spawn(Transform position, Animator animator)
    {
        GenerateWeaponStats();
        
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
    
    private void GenerateWeaponStats()
    {
        if(_lookThroughWeapon != null) return;

        _lookThroughWeapon = new Dictionary<WeaponEnum, Dictionary<WeaponStatsEnum, int[]>>();

        foreach (var weaponClass in _weaponClasses)
        {
            var lookThroughWeaponStats = new Dictionary<WeaponStatsEnum, int[]>();

            foreach (var weaponClassStat in weaponClass.stats)
            {
                lookThroughWeaponStats[weaponClassStat.weaponStats] = weaponClassStat.rarities;
            }

            _lookThroughWeapon[weaponClass.weapon] = lookThroughWeaponStats;
        }
    }

   
    public GameObject GetProjectile()
    {
        return _projectile;
    }

    public Item.ItemType GetItemType()
    {
        return _itemType;
    }

    [Serializable]
    class ProgressionWeaponClass
    {
        public WeaponEnum weapon;
        public ProgressionWeaponStats[] stats;
    }
    
    [Serializable]
    class ProgressionWeaponStats
    {
        public WeaponStatsEnum weaponStats;
        public int[] rarities;
    }

    
}
