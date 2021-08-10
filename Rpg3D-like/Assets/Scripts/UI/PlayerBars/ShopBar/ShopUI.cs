using System;
using System.Collections;
using System.Collections.Generic;
using Inventories;
using Shops;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ShopUI
{
    public class ShopUI : MonoBehaviour
    {
        [SerializeField] private Transform _listRoot;
        [SerializeField] private RowUI _rowPrefab;
        [SerializeField] private TextMeshProUGUI _totalPrice;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _switcherState;

        private Customer _customer;
        private ShopSystem _shop;
        private Color _originalTotalColor;
        private void Awake()
        {
            _customer = FindObjectOfType<Customer>();
            
            if(_customer == null) return;

            _customer.OnActiveShopChange += ShopChanged;
        }

        private void Start()
        {
            _originalTotalColor = Color.white;
            _confirmButton.onClick.AddListener(ConfirmTransaction);
            _switcherState.onClick.AddListener(SwitchMode);
        }

        private void ShopChanged()
        {
            if(_shop!= null)
                _shop.OnChanged -= RefreshUI;

            _shop = _customer.GetCurrentShop;
            
            gameObject.SetActive(_customer!= null);

            foreach (FilterButton button in GetComponentsInChildren<FilterButton>())
            {
                button.SetShop(_shop);
                button.RefreshUI();
            }
            if(_shop == null) return;

            _shop.OnChanged += RefreshUI;
            
            RefreshUI();
        }

        private void RefreshUI()
        {
            foreach (Transform child in _listRoot.transform)
            {
                Destroy(child.gameObject);
            }
            
            foreach (var shopItem in _shop.GetFilteredItems())
            {
                RowUI rowUI = Instantiate(_rowPrefab, _listRoot);
                
                rowUI.SetUp(_shop, shopItem);
            }

            foreach (FilterButton button in GetComponentsInChildren<FilterButton>())
            {
                button.RefreshUI();
            }
            
            _totalPrice.text = _shop.HasEnoughPlace() ? $"Total: {_shop.TransactionTotal()}" : "Full inventory";
     
            _totalPrice.color = !_shop.HasEnoughGold() || !_shop.HasEnoughPlace() ? Color.red  : Color.white;
            _confirmButton.interactable = _shop.CanBuy();
            TextMeshProUGUI switchText = _switcherState.GetComponentInChildren<TextMeshProUGUI>();
            TextMeshProUGUI confirmText = _confirmButton.GetComponentInChildren<TextMeshProUGUI>();
            
            if (_shop.IsBuyingMode())
            {
                switchText.text = "Sell";
                confirmText.text = "Buy";
            }
            else
            {
                
                switchText.text = "Buy";
                confirmText.text = "Sell";
            }
        }

        public void ConfirmTransaction()
        {
            _shop.ConfirmTransaction();
        }

        public void SwitchMode()
        {
            _shop.SelectMode(!_shop.IsBuyingMode());
        }
    }
}