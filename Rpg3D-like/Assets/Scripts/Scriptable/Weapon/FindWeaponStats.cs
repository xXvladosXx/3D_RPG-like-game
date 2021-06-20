using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable.Weapon;
using UnityEngine;
using Random = UnityEngine.Random;

public class FindWeaponStats : MonoBehaviour
{
    [SerializeField] private WeaponScriptable _weaponScriptable;
    [SerializeField] private WeaponEnum _weaponEnum;

    public float GetWeaponStat(WeaponStatsEnum weaponStatsEnum)
    {
        return _weaponScriptable.CalculateWeaponStat(weaponStatsEnum, _weaponEnum);
    }

  
    

    
}
