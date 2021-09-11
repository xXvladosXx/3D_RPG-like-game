using System;
using System.Linq;
using UnityEngine;

namespace Inventory
{
    public enum ItemCategory
    {
        None,
        Pants,
        Helmet,
        Chest,
        Boots,
        Weapon,
        Ring,
        Neck,
        Gloves,
        Shoulder,
        Belt,
        Potion,
    }

    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
    public class InventoryObject : ScriptableObject
    {
        public ItemDatabaseObject _database;
        public Inventory _inventory;

        public bool HasEnoughPlace()
        {
            return EmptySlot > 0;
        }

        public void AddItem(ItemData itemData, int amount)
        {
            InventorySlot inventorySlot = FindItemInInventory(itemData);

            if (!_database.Items[itemData.Id].Stackable || inventorySlot == null)
            {
                for (int i = 0; i < amount; i++)
                {
                    SetEmptySlot(itemData, 1);
                }

                return;
            }

            inventorySlot.AddAmount(amount);
        }

        private InventorySlot FindItemInInventory(ItemData itemData)
        {
            return _inventory.Items.FirstOrDefault(inventorySlot => inventorySlot.itemData.Id == itemData.Id);
        }

        public int EmptySlot
        {
            get
            {
                return _inventory.Items.Count(inventorySlot =>
                    inventorySlot.itemData.Id <= -1);
            }
        }

        public InventorySlot SetEmptySlot(ItemData itemData, int amount)
        {
            foreach (var inventorySlot in _inventory.Items)
            {
                if (inventorySlot.itemData.Id > -1) continue;
                inventorySlot.UpdateSlot(itemData, amount);
                return inventorySlot;
            }

            return null;
        }

        public void SwapItem(InventorySlot draggedItem, InventorySlot replacedItem)
        {
            if (replacedItem.CanBeReplaced(draggedItem.ItemObject) &&
                draggedItem.CanBeReplaced(replacedItem.ItemObject))
            {
                InventorySlot temp = new InventorySlot(replacedItem.itemData, replacedItem.Amount);
                replacedItem.UpdateSlot(draggedItem.itemData, draggedItem.Amount);
                draggedItem.UpdateSlot(temp.itemData, temp.Amount);
            }
        }

        public void RemoveItem(int id, int amount = 0)
        {
            if (amount == 0)
            {
                foreach (var inventorySlot in _inventory.Items)
                {
                    if (inventorySlot.itemData.Id == id)
                    {
                        inventorySlot.UpdateSlot(new ItemData(), 0);
                        break;
                    }
                }
            }
            else
            {
                foreach (var inventorySlot in _inventory.Items)
                {
                    if (inventorySlot.itemData.Id == id)
                    {
                        inventorySlot.Amount -= amount;
                        break;
                    }
                }
            }
        }
    }

    [Serializable]
    public class Inventory
    {
        public InventorySlot[] Items = new InventorySlot[18];
    }

    [System.Serializable]
    public class InventorySlot
    {
        public ItemCategory[] AllowedItems = new ItemCategory[0];
        public UserInterface Parent;
        public ItemData itemData;
        public int Amount;

        public InventorySlot()
        {
            itemData = null;
            Amount = 0;
        }

        public InventorySlot(ItemData itemData, int amount)
        {
            this.itemData = itemData;
            Amount = amount;
        }

        public void UpdateSlot(ItemData itemData, int amount)
        {
            this.itemData = itemData;
            Amount = amount;
        }

        public void AddAmount(int value)
        {
            Amount += value;
        }

        public bool CanBeReplaced(ItemObject item)
        {
            if (AllowedItems.Length <= 0 || item == null || item.Data.Id < 0)
            {
                return true;
            }

            var smt = AllowedItems.Any(t => item.Category == t);

            return smt;
        }

        public void RemoveItem()
        {
            itemData = new ItemData();
            Amount = 0;
        }

        public ItemObject ItemObject
        {
            get
            {
                if (itemData.Id >= 0)
                {
                    if (Parent == null) return null;
                    
                    return Parent.InventoryObject._database.GetItem[itemData.Id];
                }

                return null;
            }
        }
    }
}