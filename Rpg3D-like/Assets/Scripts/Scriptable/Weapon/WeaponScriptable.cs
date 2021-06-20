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
    [SerializeField] private int _minStatValue;
    [SerializeField] private int _maxStatValue;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Sprite _weaponSprite;
    [SerializeField] private Item.ItemType _itemType;
    [SerializeField] private Skill[] _weaponSkills;
    [SerializeField] private ParticleSystem[] _raritiesEffects;
    [SerializeField] private int _rarity;

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

    public float CalculateWeaponStat(WeaponStatsEnum weaponStatsEnum, WeaponEnum weaponEnum)
    {
        GenerateWeaponStats();
        
        int[] rarities = _lookThroughWeapon[weaponEnum][weaponStatsEnum];

        return rarities[GenerateRarity() - 1];
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

    public void GetRarirty()
    {
        _rarity = GenerateRarity();

        foreach (var _raritiesEffect in _raritiesEffects)
        {
            if (_rarity == int.Parse(_raritiesEffect.gameObject.name))
            {
                Instantiate(_raritiesEffect, _prefabWeapon.transform.position,  Quaternion.identity);
            }
        }
    }
    
    public int GenerateRarity()
    {
        int choosenValue = 0;
        
        choosenValue = Random.Range(_minStatValue, _maxStatValue);

        return choosenValue;
    }
    
    public GameObject GetProjectile()
    {
        return _projectile;
    }

    public Item.ItemType GetItemType()
    {
        return _itemType;
    }

    public Skill[] GetWeaponSkills()
    {
        return _weaponSkills;
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
