using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class Inventory
{
    private List<Item> _inventory;
    private Action<Item> _useItemAction;

    public List<Item> GetInventory => _inventory;
    public event EventHandler OnInventoryChanged;
    public Inventory(Action<Item> useItemAction)
    {
        _inventory = new List<Item>();
        _useItemAction = useItemAction;
        
        AddItem(new Item{itemType = Item.ItemType.HealthPotion, amount = 3});
    }
    
    public void AddItem(Item item, int amount = 1)
    {
        if (item.IsStackable())
        {
            bool isInInventory = false;
            foreach (Item tempItem in _inventory)
            {
                if (tempItem.itemType == item.itemType)
                {
                    tempItem.amount += item.amount;
                    isInInventory = true;
                }
            }

            if (!isInInventory)
            {
                _inventory.Add(item);
            }
        }
        else
        {
            _inventory.Add(item);
        }

        OnInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ShowInventory()
    {
        foreach (var item in _inventory)
        {
            Debug.Log(item.itemType + item.amount);
        }
    }

    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item isInInventory = null;
            foreach (Item tempItem in _inventory)
            {
                if (tempItem.itemType == item.itemType)
                {
                    tempItem.amount --;
                    isInInventory = item;
                }
            }
            if (isInInventory != null && isInInventory.amount <= 0)
            {
                _inventory.Remove(isInInventory);
            }
        }
        else
        {
            _inventory.Remove(item);
        }

        OnInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem(Item item)
    {
        _useItemAction(item);
        RemoveItem(item);
    }
    
    public void UsePotion(PotionEnum potion, Health health)
    {
        Debug.Log("UsedPotion");
        health.RegenerateHealth();
    }

    public void EquipWeapon(WeaponEnum weapon)
    {
        
    }
}
