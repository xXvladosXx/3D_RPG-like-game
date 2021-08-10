using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Controller;
using Inventories;
using UI.Cursor;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour, IRaycastable
{
    [SerializeField] private WeaponScriptable _weapon;
    [SerializeField] private bool _isRightHanded = false;
    [SerializeField] private Item _item;
    public Item GetItem => _item;

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
            playerController.GetComponent<Combat>().EquipWeapon(_weapon);
            playerController.GetComponent<PlayerInventory>().InventoryPlacerItem(_item.ItemType);

            OnWeaponPicked?.Invoke();

            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
            Destroy(gameObject);
        }
    }

    public PlayerController.CursorType GetCursorType()
    {
        return PlayerController.CursorType.PickUp;
    }

    public bool HandleRaycast(PlayerController player)
    {
        if (Input.GetMouseButton(0))
        {
            player.GetComponent<Movement>().StartMoveToAction(gameObject.transform.position, 1f);
        }
        return true;
    }
}
