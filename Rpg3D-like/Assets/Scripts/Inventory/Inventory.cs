using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Enums;
using Stats;
using UnityEngine;

namespace Inventories
{

public class Inventory 
{
    private List<Item> _inventory;
    private Action<Item> _useItemAction;

    public List<Item> GetInventory => _inventory;

    public void SetInventory(List<Item> items)
    {
        _inventory = items;
        
    }
    public event EventHandler OnInventoryChanged;
    public Inventory(Action<Item> useItemAction = null)
    {
        _inventory = new List<Item>();
        _useItemAction = useItemAction;
    }
    
    public void AddItem(Item item, int amount = 1)
    {
        if(item== null) return;
        item.Amount = amount;
        if (item.IsStackable())
        {
            bool isInInventory = false;
            foreach (Item tempItem in _inventory)
            {
                if (item.IItem != null && tempItem.IItem != null && tempItem.IItem.GetItemType == item.IItem.GetItemType)
                {
                    tempItem.Amount += amount;
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
            for (int i = 0; i < amount; i++)
            {
                _inventory.Add(item);
            }
        }

        OnInventoryChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item, int amount = 1)
    { 
        if (item.IsStackable())
        {
            Item isInInventory = null;
            foreach (Item tempItem in _inventory)
            {
                if (tempItem.IItem.GetItemType == item.ItemType)
                {
                    tempItem.Amount -= amount;
                    isInInventory = tempItem;
                }
            }
            if (isInInventory != null && isInInventory.Amount <= 0)
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
        Debug.Log(item.ItemType);
        _useItemAction(item);
        RemoveItem(item);
    }
    
    public void UsePotion(ItemType itemType, Health health = null, Mana mana = null)
    {
        if (mana is { })
        {
            mana.RegenerateMana(20);
            UseItem(new Item{ItemType = itemType});
        }

        if (health is { })
        {
            Debug.Log("wad");
            health.RegenerateHealth(20);
            UseItem(new Item{ItemType = itemType});
        }
        
    }

}
}

