using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Saving;
using UI.Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Inventories
{
    public class PlayerInventory : MonoBehaviour, ISaveable
{
    private readonly int _inventoryCapacity = 9;
    public Inventory GetInventory { get; private set; }

    [SerializeField] private UIInventory _uiInventory;
    private void Awake()
    {
        _uiInventory = FindObjectOfType<UIInventory>();
        
        GetInventory = new Inventory(UseItem);
    }

    private void Start()
    {        
        _uiInventory.SetInventory(GetInventory);
    }

    public bool HasEnoughPlace(int amountToLocate = 0)
    {
        return GetInventory.GetInventory.Count + amountToLocate < _inventoryCapacity;
    }
    private void UseItem(Item item)
    {
        if (item.IItem != null) item.IItem.UseItem(gameObject, item);
    }
 
    public void InventoryPlacerItem(ItemType item, int amount = 1)
    {
        GetInventory.AddItem(ItemsSpawnManager.Instance.ItemTypeSwitcher(item), amount);
    }

    
    public void DropItem(Item item, Vector3 playerPositionPosition)
    {
        Vector3 randomDirection = new Vector3(playerPositionPosition.x + 1, playerPositionPosition.y, playerPositionPosition.z+1);
/*
        switch (item._item)
        {
            case Item.ItemType.HealthPotion: ItemsSpawnManager.Instance.SpawnItem(item, randomDirection);
                break;
            case Item.ItemType.Gold: ItemsSpawnManager.Instance.SpawnItem(item, randomDirection);
                break;
        }*/   
    }

    public object CaptureState()
    {
        List<ItemType> items = new List<ItemType>();

        foreach (var item in GetInventory.GetInventory)
        {
            while (item.Amount != 0)
            {
                items.Add(item.IItem.GetItemType);
                item.Amount--;
            }
        }
        
        return items;
    }

    public void RestoreState(object state)
    {
        GetInventory.GetInventory.Clear();
        foreach (var itemType in (List<ItemType>) state)
        {
            InventoryPlacerItem(itemType);
        }
    }
}

}
