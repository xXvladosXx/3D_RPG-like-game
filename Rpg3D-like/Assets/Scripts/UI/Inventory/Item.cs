using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item 
{
    public enum ItemType
    {
        Sword,
        Bow,
        HealthPotion,
        Gold
    }


    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.Sword:  return false;
            case ItemType.Bow:  return false;
            case ItemType.HealthPotion:  return true;
            case ItemType.Gold:    return true; 
        }
    }
    
    
    public ItemType itemType ;
    public int amount;

    public Sprite GetItemSprite()
    {
        switch (itemType)
        {
            default:
                case ItemType.Sword: return  ItemsSprite.Instance.SwordSprite;
                case ItemType.Bow: return  ItemsSprite.Instance.BowSprite;
                case ItemType.HealthPotion: return ItemsSprite.Instance.HealthPotionSprite;
                case ItemType.Gold:   return ItemsSprite.Instance.GoldSprite;
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

