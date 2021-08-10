using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Inventories;
using Saving;
using UnityEngine;
using UnityEngine.UI;

namespace Shops{
    public class ShopSystem : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _sellingModifier = 50f;
        [SerializeField] private ShopConfigItem[] _shopConfigItems;

        private Inventory _sellerInventory;
        private ItemCategory _itemCategory = ItemCategory.None;

        private Dictionary<ItemType, int> _transaction = new Dictionary<ItemType, int>();
        private Dictionary<ItemType, int> _stock = new Dictionary<ItemType, int>();
        private Customer _customer;
        private bool _buyingState = true;
        
        public event Action OnChanged;

        [Serializable]
        class ShopConfigItem
        {
            public int Availability;
            public Item Item;
            public int Price;
            public int ItemLevelAvailability;
        }

        private void Awake()
        {
            _sellerInventory = new Inventory();
            foreach (var shopItem in _shopConfigItems)
            {
                _stock[shopItem.Item.ItemType] = shopItem.Availability;
                _sellerInventory.AddItem(shopItem.Item, shopItem.Availability);
            }
        }

        public IEnumerable<ShopItem> GetFilteredItems()
        {
            foreach (var shopItem in GetAllItems())
            {
                if (shopItem.GetItem.GetItemCategory != _itemCategory 
                    && _itemCategory != ItemCategory.None) 
                    continue;
                
                yield return shopItem;
            }
        }

        public void AddToTransaction(ItemType item, int amount)
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
            PlayerInventory playerInventory = _customer.GetComponent<PlayerInventory>();
            Gold playerGold = _customer.GetComponent<Gold>();
            if(playerInventory == null) return;

            foreach (ShopItem shopItem in GetAllItems())
            {
                ItemType item = shopItem.GetItem.GetItemType;
                int amount = shopItem.GetAmount;
                float price = shopItem.GetPrice;

                for (int i = 0; i < amount; i++)
                {
                    if (_buyingState)
                    {
                        BuyItem(playerGold, price, playerInventory, item);
                    }
                    else
                    {
                        SellItem(playerGold, price, playerInventory, item);
                    }
                }
            }
            
            OnChanged?.Invoke();
        }

        private void SellItem(Gold playerGold, float price, PlayerInventory playerInventory, ItemType item)
        {
            AddToTransaction(item, -1);
            playerInventory.GetInventory.RemoveItem(new Item{ItemType = item});
            _stock[item]++;
            playerGold.UpdateGold(price);
        }

        private void BuyItem(Gold playerGold, float price, PlayerInventory playerInventory, ItemType item)
        {
            if(playerGold.GetGold < price) return;
            
            bool hasEnoughPlace = playerInventory.HasEnoughPlace();
            if (!hasEnoughPlace) return;

            AddToTransaction(item, -1);
            _stock[item]--;
            playerInventory.InventoryPlacerItem(item);
            playerGold.UpdateGold(-price);
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            foreach (var shopConfigItem in _shopConfigItems)
            {
                _transaction.TryGetValue(shopConfigItem.Item.ItemType, out var amount);
                
                var itemAvailability = GetItemAvailability(shopConfigItem.Item.ItemType);

                if (IsBuyingMode())
                {
                    if (GetCustomerLevel() > shopConfigItem.ItemLevelAvailability)
                    {
                        yield return new ShopItem(
                            ItemsSpawnManager.Instance.ItemTypeSwitcher(shopConfigItem.Item.ItemType),
                            itemAvailability,
                            GetPrice(shopConfigItem), amount);
                    }
                }
                else
                {
                        yield return new ShopItem(
                            ItemsSpawnManager.Instance.ItemTypeSwitcher(shopConfigItem.Item.ItemType),
                            itemAvailability,
                            GetPrice(shopConfigItem), amount);
                }
            }
        }

        private int GetItemAvailability(ItemType item)
        {
            if (_buyingState)
            {
                int itemAvailability = _stock[item];
                return itemAvailability;
            }

            return CountItemsInInventory(item);
        }

        private int CountItemsInInventory(ItemType itemType)
        {
            PlayerInventory playerInventory = _customer.GetComponent<PlayerInventory>();
            if (playerInventory == null) return 0;

            int total = 0;
            foreach (var item in playerInventory.GetInventory.GetInventory)
            {
                if (item.IItem.GetItemType == itemType)
                {
                    total += item.Amount;
                }
                
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
            if (_shopConfigItems.Any(shopItem => shopItem.Item.IsStackable()))
            {
                return true;
            }

            return GetAllItems().Select(shopItem => shopItem.GetAmount)
                .Select(amount => _customer.GetComponent<PlayerInventory>()
                    .HasEnoughPlace(amount))
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
            return _stock;
        }

        public void RestoreState(object state)
        {
            _stock = new Dictionary<ItemType, int>((Dictionary<ItemType, int>) state);
        }
    }
}