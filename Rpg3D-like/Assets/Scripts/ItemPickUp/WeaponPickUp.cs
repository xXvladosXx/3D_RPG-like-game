using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Controller;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour, IClickable
{
    // Start is called before the first frame update

    [SerializeField] private WeaponScriptable _weapon;
    [SerializeField] private bool _isRightHanded = false;

    private Collider _collider;
    private FindWeaponStats _findWeaponStats;
    public event Action OnWeaponPicked;
    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _findWeaponStats = GetComponent<FindWeaponStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            other.GetComponent<Combat>().EquipWeapon(_weapon, _isRightHanded, this,_findWeaponStats.GetWeaponStat(WeaponStatsEnum.Damage),
                _findWeaponStats.GetWeaponStat(WeaponStatsEnum.AttackRadius));
            
            other.GetComponent<PlayerSkills>().SetPlayerSkills(_weapon.GetWeaponSkills());
            if (OnWeaponPicked != null) OnWeaponPicked();

            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
            Destroy(gameObject);
        }
    }


    public void OnHoverEnter()
    {
        GetComponentInChildren<Renderer>().material.SetFloat("_Outline", 0.1f);
    }

    public void OnHoverExit()
    {
        GetComponentInChildren<Renderer>().material.SetFloat("_Outline", 0);
    }
}
