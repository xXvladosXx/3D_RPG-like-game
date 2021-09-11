using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Inventory;
using SavingSystem;
using Scriptable.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace Shops{
    public class ShopSystem : MonoBehaviour, ISaveable
    {
        [SerializeField] private ItemDatabaseObject _database;
        [SerializeField] private float _sellingModifier = 50f;
        [SerializeField] private ShopConfigItem[] _shopConfigItems;
        
        private ItemCategory _itemCategory = ItemCategory.None;

        private Dictionary<ItemObject, int> _transaction = new Dictionary<ItemObject, int>();
        private Dictionary<ItemObject, int> _stock = new Dictionary<ItemObject, int>();
        private Customer _customer;
        private bool _buyingState = true;
        
        public event Action OnChanged;

        [Serializable]
        class ShopConfigItem
        {
            public int Availability;
            public int ItemID;
            public int Price;
            public int ItemLevelAvailability;
        }

        private void Awake()
        {
            foreach (var shopItem in _shopConfigItems)
            {
                _stock[_database.GetItem[shopItem.ItemID]] = shopItem.Availability;
            }
        }

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            foreach (var shopItem in GetAllItems())
            {
                if (shopItem.GetItem.Category != _itemCategory 
                    && _itemCategory != ItemCategory.None) 
                    continue;
                
                yield return shopItem;
            }
        }

        public void AddToTransaction(ItemObject item, int amount)
        {
            if (!_transaction.ContainsKey(item))
            {
                _transaction[item] = 0;
            }

            int availability = GetItemAvailability(item);
            if (_transaction[item] + amount > availability)
            {
                _transaction[item] = availability;
            }
            else
            {
                _transaction[item] += amount;
            }

            if (_transaction[item] <= 0)
            {
                _transaction.Remove(item);
            }

            OnChanged?.Invoke();
        }

        public void SetCustomer(Customer customer)
        {
            _customer = customer;
        }

        public void ConfirmTransaction()
        {
            PlayerInventory inventory = _customer.GetComponent<PlayerInventory>();
            Gold playerGold = _customer.GetComponent<Gold>();
            if(inventory == null) return;

            foreach (ShopItem shopItem in GetAllItems())
            {
                ItemObject item = shopItem.GetItem;
                int amount = shopItem.GetAmount;
                float price = shopItem.GetPrice;

                for (int i = 0; i < amount; i++)
                {
                    if (_buyingState)
                    {
                        BuyItem(playerGold, price, inventory.InventoryObject, item);
                    }
                    else
                    {
                        SellItem(playerGold, price, inventory.InventoryObject, item);
                    }
                }
            }
            
            OnChanged?.Invoke();
        }

        private void SellItem(Gold playerGold, float price, InventoryObject inventory, ItemObject item)
        {
            AddToTransaction(item, -1);
            inventory.RemoveItem(item.Data.Id, 1);
            _stock[item]++;
            playerGold.UpdateGold(price);
            
            foreach (var shopConfigItem in _shopConfigItems)
            {
                if(shopConfigItem.ItemID == item.Data.Id)
                    shopConfigItem.Availability = _stock[item];
            }
        }

        private void BuyItem(Gold playerGold, float price, InventoryObject inventory, ItemObject item)
        {
            if(playerGold.GetGold < price) return;
            
            bool hasEnoughPlace = inventory.HasEnoughPlace();
            if (!hasEnoughPlace) return;

            AddToTransaction(item, -1);
            _stock[item]--;
            inventory.AddItem(item.Data, 1);
            playerGold.UpdateGold(-price);

            foreach (var shopConfigItem in _shopConfigItems)
            {
                if(shopConfigItem.ItemID == item.Data.Id)
                    shopConfigItem.Availability = _stock[item];
            }
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            foreach (var shopConfigItem in _shopConfigItems)
            {
                _transaction.TryGetValue(_database.GetItem[shopConfigItem.ItemID], out var amount);
                
                var itemAvailability = GetItemAvailability(_database.GetItem[shopConfigItem.ItemID]);

                if (IsBuyingMode())
                {
                    if (GetCustomerLevel() > shopConfigItem.ItemLevelAvailability)
                    {
                        yield return new ShopItem(
                            _database.GetItem[shopConfigItem.ItemID],
                            itemAvailability,
                            GetPrice(shopConfigItem), amount);
                    }
                }
                else
                {
                        yield return new ShopItem(
                            _database.GetItem[shopConfigItem.ItemID],
                            itemAvailability,
                            GetPrice(shopConfigItem), amount);
                }
            }
        }

        private int GetItemAvailability(ItemObject item)
        {
            if (_buyingState)
            {
                int itemAvailability = _stock[item];
                return itemAvailability;
            }

            return CountItemsInInventory(item);
        }

        private int CountItemsInInventory(ItemObject item)
        {
            PlayerInventory inventory = _customer.GetComponent<PlayerInventory>();
            if (inventory == null) return 0;

            int total = 0;
            foreach (var inventorySlot in inventory.InventoryObject._inventory.Items)
            {
                if(inventorySlot.itemData.Id == item.Data.Id)
                    total += inventorySlot.Amount;
            }

            return total;
        }

        private int GetPrice(ShopConfigItem shopConfigItem)
        {
            if (!_buyingState)
            {
                return shopConfigItem.Price;
            }
            
            return (int) (shopConfigItem.Price * (100 / _sellingModifier));
        }

        public void SelectMode(bool isBuying)
        {
            _buyingState = isBuying;

            OnChanged?.Invoke();
        }

        public bool IsBuyingMode()
        {
            return _buyingState;
        }

        public bool CanBuy()
        {
            if (_transaction.Count == 0)
                return false;

            return HasEnoughGold() && HasEnoughPlace();
        }

        public bool HasEnoughGold()
        {
            return TransactionTotal() <= _customer.GetComponent<Gold>().GetGold;
        }

        public bool HasEnoughPlace()
        {
            if (_shopConfigItems.Any(shopItem => _database.GetItem[shopItem.ItemID].Stackable))
            {
                return true;
            }

            return GetAllItems().Select(shopItem => shopItem.GetAmount)
                .Select(amount => _customer.GetComponent<PlayerController>())
                .All(hasEnoughPlace => hasEnoughPlace);
        }

        public float TransactionTotal()
        {
            float total = 0;
            foreach (var shopItem in GetAllItems())
            {
                total += shopItem.GetPrice * shopItem.GetAmount;
            }

            return total;
        }


        public void FilteringItems(ItemCategory itemCategory)
        {
            _itemCategory = itemCategory;
            
            OnChanged?.Invoke();
        }

        public ItemCategory GetFilter()
        {
            return _itemCategory;
        }

        private int GetCustomerLevel()
        {
            var customerLevel = _customer.GetComponent<FindStat>().GetLevel();

            return customerLevel;
        }

        public object CaptureState()
        {
            foreach (var shopConfigItem in _shopConfigItems)
            {
                print(shopConfigItem.Availability);
            }
            
            return _shopConfigItems;
        }

        public void RestoreState(object state)
        {
            _shopConfigItems = (ShopConfigItem[])state;
            
            foreach (var shopConfigItem in _shopConfigItems)
            {
                print(shopConfigItem.Availability);
            }
        }
    }
}