using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using DialogueSystem;
using Inventory;
using UI.PlayerBars.UpgradeBar;
using UnityEngine;

namespace UpgradeSystem
{
    public class ItemUpgrader
    {
        private PlayerInventory _playerInventory;
        private Gold _playerGold;
        private List<ItemObject> _upgradableItems = new List<ItemObject>();

        private int _currentStep;
        private int _currentIndexItem = 0;

        public ItemUpgrader(PlayerController customer)
        {
            _playerInventory = customer.GetComponent<PlayerInventory>();
            _playerGold = customer.GetComponent<Gold>();
        }

        public void LoadItems()
        {
            _upgradableItems.Clear();
            if (_playerInventory.InventoryObject._inventory.Items.Length == 0) return;

            foreach (var inventorySlot in _playerInventory.InventoryObject._inventory.Items)
            {
                if (inventorySlot.itemData.Id <= -1) continue;

                if (inventorySlot.ItemObject is ModifiableItem && inventorySlot.ItemObject.IsUpgradable() != null)
                {
                    _upgradableItems.Add(inventorySlot.ItemObject);
                }
            }

            if(_upgradableItems.Count == 0) return;
            
            _currentIndexItem = _upgradableItems.Count-1;
        }

        public bool MoveLeft()
        {
            if (_currentIndexItem-1 < 0)
                return false;

            _currentIndexItem--;
            return _currentIndexItem != 0;
        }

        public bool MoveRight()
        {
            if (_currentIndexItem+1 > _upgradableItems.Count-1)
                return false;

            _currentIndexItem++;
            return _currentIndexItem < _upgradableItems.Count-1;
        }
        
        public void UpgradeWeapon()
        {
            ModifiableItem modifiableItem = GetCurrentItem();
            ItemObject upgradedItem = null;

            foreach (var inventorySlot in _playerInventory.InventoryObject._inventory.Items)
            {
                if (inventorySlot.ItemObject != modifiableItem) continue;
                
                _playerInventory.InventoryObject.RemoveItem(modifiableItem.Data.Id);;
                upgradedItem = modifiableItem.IsUpgradable();
                break;
            }

            if (upgradedItem != null)
            {
                _playerInventory.InventoryObject.AddItem(upgradedItem.Data, 1);
            }
        }

        public ModifiableItem GetCurrentItem()
        {
            if (_upgradableItems.Count == 0) return null;
            return _upgradableItems[_currentIndexItem] as ModifiableItem;
        }

    }
}