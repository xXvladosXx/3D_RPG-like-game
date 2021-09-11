using System;
using Inventory;
using UnityEngine;
namespace Shops
{
    public class ShopItem
    {
        private ItemObject _item;
        public ItemObject GetItem => _item;
        public string GetItemName => _item.Data.Name.ToString();
        public Sprite GetItemSprite => _item.UIDisplay;

        private int _availability;
        public int GetAvailability => _availability;
        private float _price;
        public float GetPrice => _price;

        private int _amount;
        public int GetAmount => _amount;

        public ShopItem(ItemObject item, int availability, float price, int amount)
        {
            _item = item;
            _availability = availability;
            _price = price;
            _amount = amount;
        }
    }
}