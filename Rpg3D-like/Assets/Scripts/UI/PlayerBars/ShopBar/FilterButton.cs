using Inventory;
using Shops;
using UnityEngine;
using UnityEngine.UI;

namespace UI.PlayerBars.ShopBar
{
    public class FilterButton : MonoBehaviour
    {
        [SerializeField] private ItemCategory _category = ItemCategory.None;
        private Button _button;
        private ShopSystem _shop;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(SelectFilter);
        }

        public void SetShop(ShopSystem shop)
        {
            _shop = shop;
        }

        public void RefreshUI()
        {
            _button.interactable = _shop.GetFilter() != _category;
        }
        
        private void SelectFilter()
        {
            _shop.FilteringItems(_category);
        }
    }
}