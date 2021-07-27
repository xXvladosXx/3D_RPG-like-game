using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.WSA;

[Serializable]
public class Item
{
    public enum ItemType
    {
        Sword,
        Sword1,
        Bow,
        HealthPotion,
        Gold
    }


    public enum ItemLevel
    {
        First,
        Second,
        Third
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.Sword: return false;
            case ItemType.Sword1: return false;
            case ItemType.Bow: return false;
            case ItemType.HealthPotion: return true;
            case ItemType.Gold: return true;
        }
    }


    public ItemType itemType;
    public int amount;

    public Sprite GetItemSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Sword: return ItemsSprite.Instance.SwordSprite;
            case ItemType.Sword1: return ItemsSprite.Instance.SwordSprite1;
            case ItemType.Bow: return ItemsSprite.Instance.BowSprite;
            case ItemType.HealthPotion: return ItemsSprite.Instance.HealthPotionSprite;
            case ItemType.Gold: return ItemsSprite.Instance.GoldSprite;
        }
    }

    public Sprite GetUpgradedItemSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Sword: return ItemsSprite.Instance.SwordSprite1;
            case ItemType.Sword1: return ItemsSprite.Instance.BowSprite;
            case ItemType.Bow: return ItemsSprite.Instance.BowSprite;
            case ItemType.HealthPotion: return ItemsSprite.Instance.HealthPotionSprite;
            case ItemType.Gold: return ItemsSprite.Instance.GoldSprite;
        }
    }
    
    public ItemLevel GetItemLevel()
    {
        switch (itemType)
        {
            default:
            case ItemType.Sword: return ItemLevel.First;
            case ItemType.Sword1: return ItemLevel.Second;
            case ItemType.Bow: return ItemLevel.First;
        }
    }

    public ItemLevel GetUpgradedItemLevel(ItemLevel itemLevel)
    {
        switch (itemLevel)
        {
            default:
            case ItemLevel.First: return ItemLevel.Second;
            case ItemLevel.Second: return ItemLevel.Third;
            case ItemLevel.Third: return ItemLevel.First;
        }
    }

    public Item GetUpgradedItem(Item item)
    {
        switch (itemType)
        {
            default:
                case Item.ItemType.Sword: return new Item{itemType = ItemType.Sword1};
                case ItemType.Sword1: return new Item{itemType = ItemType.Bow};
        }
    }
public string GetItemAmount()
    {
        if(IsStackable())
            return amount.ToString();
        else
            return "";
    }
}

