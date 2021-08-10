using System;
using Inventories;
using UnityEngine;

namespace Shops
{
    public class ShopItem
    {
        private Item _item;
        public IItem GetItem => _item.IItem;
        public string GetItemName => _item.IItem.GetItemType.ToString();
        public Sprite GetItemSprite => _item.GetItemSprite();

        private int _availability;
        public int GetAvailability => _availability;
        private float _price;
        public float GetPrice => _price;

        private int _amount;
        public int GetAmount => _amount;

        public ShopItem(Item item, int availability, float price, int amount)
        {
            _item = item;
            _availability = availability;
            _price = price;
            _amount = amount;
        }
    }
}