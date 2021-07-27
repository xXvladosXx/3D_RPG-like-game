using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Controller;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    [SerializeField] private WeaponScriptable _weapon;
    [SerializeField] private bool _isRightHanded = false;

    private Collider _collider;
    public event Action OnWeaponPicked;
    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            other.GetComponent<Combat>().EquipWeapon(_weapon);
            
            if (OnWeaponPicked != null) OnWeaponPicked();

            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
            Destroy(gameObject);
        }
    }

}
