using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Inventories;
using TMPro;
using UI.Cursor;
using UI.Inventory;
using UI.PlayerBars.UpgradeBar;
using UnityEngine;
using UnityEngine.UI;

namespace UpgradeSystem
{
    public class UpgradeDialogueStrategy : InitializeDialogueStrategy, IUpgradeable
    {
        [SerializeField] private GameObject _upgradeBar;

        private Inventory _playerInventory;
        private List<Item> _weaponItems;
        public List<Item> GetItems => _weaponItems;
        private Gold _playerGold;

        private int _currentStep;
        private int _currentIndexItem = 0;
        
        public override event Action OnDialogChanged;
        private void Awake()
        {
            _weaponItems = new List<Item>();

            _playerInventory = FindObjectOfType<PlayerInventory>().GetInventory;
            _playerGold = FindObjectOfType<Gold>();
        }

        public override void InitializeDialogMessage()
        {
            Upgrade();
        }

        public void Upgrade()
        {           
            _weaponItems.Clear();
            _upgradeBar.SetActive(true);
            
            OnDialogChanged?.Invoke();

            foreach (var item in _playerInventory.GetInventory.Where(item => !item.IsStackable()))
            {
                _weaponItems.Add(item);
            }
            
            _upgradeBar.GetComponent<UpgradeUI>().SetUpgradeDialog(this);
            _upgradeBar.GetComponent<UpgradeUI>().OnWeaponUpgraded += UpgradeWeapon;
        }


        private void UpgradeWeapon(Item itemToUpgrade)
        {
            if (!HasEnoughMoney(itemToUpgrade)) return;
            
            Item upgradedItem = null;
            foreach (var item in _weaponItems.Where(item => item.IItem.GetItemType == itemToUpgrade.IItem.GetItemType))
            {
                upgradedItem = item;
                _playerGold.UpdateGold(-itemToUpgrade.GetUpgradePrice());
                _playerInventory.RemoveItem(item);
                break;
            }
            
            _weaponItems.Clear();


            if (upgradedItem != null)
            {
                ItemsSpawnManager.Instance.SpawnItem(upgradedItem.GetUpgradedItem().IItem.GetItemType,
                    _playerGold.gameObject.transform.position);
            }

            _upgradeBar.SetActive(false);
            OnDialogChanged?.Invoke();
        }

        public bool HasEnoughMoney(Item itemToUpgrade)
        {
            return itemToUpgrade.GetUpgradePrice() < _playerGold.GetGold;
        }

        
    }
}
