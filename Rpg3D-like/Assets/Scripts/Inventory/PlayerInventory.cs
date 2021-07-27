using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Saving;
using UI.Inventory;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, ISaveable
{
    private Inventory _inventory;
    private int _inventoryCapacity = 9;
    public Inventory GetInventory => _inventory;
    private Health _health;
    
    [SerializeField] private UIInventory _uiInventory;
    private void Awake()
    {
        _uiInventory = FindObjectOfType<UIInventory>();
        _health = GetComponent<Health>();
        _inventory = new Inventory(UseItem);
    }

    private void Start()
    {
        _uiInventory.SetInventory(_inventory);
    }

    public bool HasEnoughPlace()
    {
        if (_inventory.GetInventory.Count >= _inventoryCapacity)
            return false;

        return true;
    }
    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion: _inventory.UsePotion(PotionEnum.Health, _health);
                break;
            case Item.ItemType.Bow: ItemsSpawnManager.Instance.SpawnItem(item, gameObject.transform.position);
                break;
            case Item.ItemType.Sword: ItemsSpawnManager.Instance.SpawnItem(item, gameObject.transform.position);
                break;
            case Item.ItemType.Sword1: ItemsSpawnManager.Instance.SpawnItem(item, gameObject.transform.position);
                break;
        }
    }
    public void InventoryPlacerWeapon(WeaponScriptable weapon)
    {
        if(weapon == null) return;

        foreach (var item in _inventory.GetInventory)
        {
            if(item.itemType == weapon.GetItemType()) return;
        }

        _inventory.AddItem(new Item {itemType = weapon.GetItemType(),amount = 1});
    }

    public void InventoryPlacerItem(Item.ItemType item, int amount = 1)
    {
        _inventory.AddItem(new Item{itemType = item, amount = amount});
    }


    public void DropItem(Item item, Vector3 playerPositionPosition)
    {
        Vector3 randomDirection = new Vector3(playerPositionPosition.x + 1, playerPositionPosition.y, playerPositionPosition.z+1);

        switch (item.itemType)
        {
            case Item.ItemType.HealthPotion: ItemsSpawnManager.Instance.SpawnItem(item, randomDirection);
                break;
            case Item.ItemType.Gold: ItemsSpawnManager.Instance.SpawnItem(item, randomDirection);
                break;
        }   
    }

    public object CaptureState()
    {
        return _inventory.GetInventory;
    }

    public void RestoreState(object state)
    {
        _inventory.SetInventory(new List<Item>((List<Item>)state));
    }
}
