using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UI.Inventory;
using UnityEngine;

namespace Inventories
{
    public interface IItemType
{
    ItemType GetItemType { get; }
}

public interface IItemCategory : IItemType
{
    ItemCategory GetItemCategory { get; }
}
public interface IItem : IItemCategory
{
    void UseItem(GameObject user, Item item);
    Sprite GetItemSprite { get; }
}

public interface IUpgradeableItem : IItem
{
    IUpgradeableItem Upgraded();
    int UpgradePrice();
}


public class Sword0 : IUpgradeableItem
{
    public ItemType GetItemType { get; } = ItemType.Sword0;
    public IUpgradeableItem Upgraded() => new Sword1();
    public int UpgradePrice()
    {
        return 10;
    }

    public void UseItem(GameObject user, Item item)
    {
        ItemsSpawnManager.Instance.SpawnItem(GetItemType, user.transform.position);
    }
    public Sprite GetItemSprite { get; } = ItemsSprite.Instance.SwordSprite0;
    public ItemCategory GetItemCategory { get; } = ItemCategory.Weapon;
}

public class Sword1 : IUpgradeableItem
{
    public IUpgradeableItem Upgraded() => new Sword2();
    public int UpgradePrice()
    {
        return 20;
    }

    public ItemType GetItemType { get; } = ItemType.Sword1;
    public void UseItem(GameObject user,Item item)
    {
        ItemsSpawnManager.Instance.SpawnItem(GetItemType, user.transform.position);
    }

    public Sprite GetItemSprite { get; } = ItemsSprite.Instance.SwordSprite1;
    public ItemCategory GetItemCategory { get; } = ItemCategory.Weapon;
}

public class Sword2 : IUpgradeableItem
{
    public IUpgradeableItem Upgraded() => new Sword3();
    public int UpgradePrice()
    {
        return 30;
    }

    public ItemType GetItemType { get; } = ItemType.Sword2;

    public void UseItem(GameObject user,Item item)
    {
        ItemsSpawnManager.Instance.SpawnItem(GetItemType, user.transform.position);
    }

    public Sprite GetItemSprite { get; } = ItemsSprite.Instance.SwordSprite2;

    public ItemCategory GetItemCategory { get; } = ItemCategory.Weapon;
}
public class Sword3 : IUpgradeableItem
{
    public IUpgradeableItem Upgraded() => new Sword4();
    public int UpgradePrice()
    {
        return 40;
    }

    public ItemType GetItemType { get; } = ItemType.Sword3;

    public void UseItem(GameObject user,Item item)
    {
        ItemsSpawnManager.Instance.SpawnItem(GetItemType, user.transform.position);
    }

    public Sprite GetItemSprite { get; } = ItemsSprite.Instance.SwordSprite3;

    public ItemCategory GetItemCategory { get; } = ItemCategory.Weapon;
}
public class Sword4 : IUpgradeableItem
{
    public IUpgradeableItem Upgraded() => new Bow0();
    public int UpgradePrice()
    {
        return 50;
    }

    public ItemType GetItemType { get; } = ItemType.Sword4;

    public void UseItem(GameObject user,Item item)
    {
        ItemsSpawnManager.Instance.SpawnItem(GetItemType, user.transform.position);
    }

    public Sprite GetItemSprite { get; } = ItemsSprite.Instance.SwordSprite4;

    public ItemCategory GetItemCategory { get; } = ItemCategory.Weapon;
}

[Serializable]
public class Bow0 : IUpgradeableItem
{
    public IUpgradeableItem Upgraded() => new Bow1();
    public int UpgradePrice()
    {
        return 10;
    }

    public ItemType GetItemType { get; } = ItemType.Bow0;

    public void UseItem(GameObject user, Item item)
    {
        ItemsSpawnManager.Instance.SpawnItem(GetItemType, user.transform.position);
    }

    public Sprite GetItemSprite { get; } = ItemsSprite.Instance.BowSprite0;

    public ItemCategory GetItemCategory { get; } = ItemCategory.Weapon;
}
public class Bow1 : IUpgradeableItem
{
    public IUpgradeableItem Upgraded() => new Bow2();
    public int UpgradePrice()
    {
        return 20;
    }

    public ItemType GetItemType { get; } = ItemType.Bow1;
    public void UseItem(GameObject user, Item item)
    {
        ItemsSpawnManager.Instance.SpawnItem(GetItemType, user.transform.position);
    }

    public Sprite GetItemSprite { get; } = ItemsSprite.Instance.BowSprite1;
    public ItemCategory GetItemCategory { get; } = ItemCategory.Weapon;
}
public class Bow2 : IUpgradeableItem
{
    public IUpgradeableItem Upgraded() => new Bow3();
    public int UpgradePrice()
    {
        return 30;
    }

    public ItemType GetItemType { get; } = ItemType.Bow2;
    public void UseItem(GameObject user, Item item)
    {
        ItemsSpawnManager.Instance.SpawnItem(GetItemType, user.transform.position);
    }

    public Sprite GetItemSprite { get; } = ItemsSprite.Instance.BowSprite2;
    public ItemCategory GetItemCategory { get; } = ItemCategory.Weapon;
}
public class Bow3 : IUpgradeableItem
{
    public IUpgradeableItem Upgraded() => new Bow4();
    public int UpgradePrice()
    {
        return 40;
    }

    public ItemType GetItemType { get; } = ItemType.Bow3;
    public void UseItem(GameObject user, Item item)
    {
        ItemsSpawnManager.Instance.SpawnItem(GetItemType, user.transform.position);
    }

    public Sprite GetItemSprite { get; } = ItemsSprite.Instance.BowSprite3;

    public ItemCategory GetItemCategory { get; } = ItemCategory.Weapon;
}
public class Bow4 : IUpgradeableItem
{
    public IUpgradeableItem Upgraded() => new Sword0();
    public int UpgradePrice()
    {
        return 50;
    }

    public ItemType GetItemType { get; } = ItemType.Bow4;

    public void UseItem(GameObject user, Item item)
    {
        ItemsSpawnManager.Instance.SpawnItem(GetItemType, user.transform.position);
    }

    public Sprite GetItemSprite { get; } = ItemsSprite.Instance.BowSprite4;
    public ItemCategory GetItemCategory { get; } = ItemCategory.Weapon;
}

public class HealthPotion : IItem
{
    public ItemType GetItemType { get; }= ItemType.HealthPotion;

    public void UseItem(GameObject user, Item item)
    {
        user.GetComponent<PlayerInventory>().GetInventory.UsePotion(GetItemType, user.GetComponent<Health>());
    }
    public Sprite GetItemSprite { get; } = ItemsSprite.Instance.HealthPotionSprite;

    public ItemCategory GetItemCategory { get; } = ItemCategory.Potion;
}


[Serializable]
public class Item 
{
    [SerializeField] public ItemType ItemType;
    [CanBeNull] public IItem IItem;
    public int Amount;
    public bool IsStackable() => IItem is IUpgradeableItem is false;
    
    public Sprite GetItemSprite()
    {
        if (IItem != null) return IItem.GetItemSprite;
        
        return null;
    }

    public Sprite GetUpgradedItemSprite()
    {
        if (IItem is IUpgradeableItem upgradeableItem)
        {
            return upgradeableItem.Upgraded().GetItemSprite;
        }

        return IItem.GetItemSprite;
    }

    [CanBeNull]
    public ItemType GetUpgradedItemType()
        => IItem is IUpgradeableItem upgradeableItem ? upgradeableItem.GetItemType : ItemType.HealthPotion;

    public int GetUpgradePrice() => IItem is IUpgradeableItem upgradeableItem ? upgradeableItem.UpgradePrice() : 0;

    
    public Item GetUpgradedItem()
    {
        IItem item = IItem is IUpgradeableItem upgradeableItem ?
            upgradeableItem.Upgraded() :
            null;

        return new Item{IItem = item};
    }

    public string GetItemAmount() 
        => IsStackable() ? 
            Amount.ToString() : 
            "";
}
}
