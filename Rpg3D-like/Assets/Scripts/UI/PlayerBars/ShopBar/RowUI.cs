using System.Globalization;
using Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PlayerBars.ShopBar
{
    public class RowUI : MonoBehaviour
    {
        [SerializeField] private Image _itemImage;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemAvailability;
        [SerializeField] private TextMeshProUGUI _itemPrice;
        [SerializeField] private TextMeshProUGUI _itemAmount;
        [SerializeField] private ShopItem _shopItem;

        private ShopSystem _shop;
        public void SetUp(ShopSystem shopSystem, ShopItem shopItem)
        {
            _shop = shopSystem;
            _shopItem = shopItem;
            _itemName.text = shopItem.GetItemName;
            _itemAvailability.text = shopItem.GetAvailability.ToString();
            _itemImage.sprite = shopItem.GetItemSprite;
            _itemPrice.text = shopItem.GetPrice.ToString(CultureInfo.InvariantCulture);
            _itemAmount.text = shopItem.GetAmount.ToString();
        }

        public void Add()
        {
            _shop.AddToTransaction(_shopItem.GetItem, 1);
        }

        public void Remove()
        {
            _shop.AddToTransaction(_shopItem.GetItem,-1);
        }
    }
}