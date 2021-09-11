using System;
using Controller;
using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UpgradeSystem;

namespace UI.PlayerBars.UpgradeBar
{
    public class UpgradeUI : MonoBehaviour
    {
        [SerializeField] private Button _moveRight;
        [SerializeField] private Button _moveLeft;
        [SerializeField] private Button _upgrade;
        [SerializeField] private TextMeshProUGUI _itemNameToUpgrade;
        [SerializeField] private TextMeshProUGUI _priceToUpgrade;
        [SerializeField] private ItemRenderer _item;
        [SerializeField] private ItemRenderer _upgradedItem;
        
        [SerializeField] private Image _itemImage;
        [SerializeField] private Image _upgradedItemImage;

        private ItemUpgrader _itemUpgrader;
        private bool _canMoveLeft;
        private bool _canMoveRight;
        private PlayerController _playerController;
        
        private void Awake()
        {
            _playerController = FindObjectOfType<PlayerController>();
        }

        private void Start()
        {
            _itemUpgrader = new ItemUpgrader(FindObjectOfType<PlayerController>());
            _itemUpgrader.LoadItems();

            UpdateUI();
            _moveRight.interactable = false;
            
            if (_itemUpgrader.GetCurrentItem() == null)
            {
                return;
            }
            
            _moveRight.onClick.AddListener(() =>
            {   
                var canMoveRight = _itemUpgrader.MoveRight();
                _canMoveRight = canMoveRight;

                if (_canMoveRight)
                {
                    _canMoveLeft = true;
                    _moveLeft.interactable = true;
                }

                UpdateUI();

                _moveRight.interactable = _canMoveRight;
            });
            
            _moveLeft.onClick.AddListener(() =>
            {
                var canMoveLeft = _itemUpgrader.MoveLeft();
                _canMoveLeft = canMoveLeft;

                if (_canMoveLeft)
                {
                    _canMoveRight = true;
                    _moveRight.interactable = true;
                }
                
                UpdateUI();

                _moveLeft.interactable = _canMoveLeft;
            });
            
            _upgrade.onClick.AddListener(() =>
            {
                _itemUpgrader.UpgradeWeapon();
                _canMoveLeft = true;
                _moveLeft.interactable = true;
                _itemUpgrader.LoadItems();

                UpdateUI();
            });
        }

        private void UpdateUI()
        {
            _item.Item = _itemUpgrader.GetCurrentItem();
            
            if (_item.Item == null)
            {
                _moveLeft.interactable = false;
                _moveRight.interactable = false;
                _upgrade.interactable = false;
                
                _itemImage.sprite = null;

                _upgradedItem.Item = null;
                _upgradedItemImage.sprite = null;

                return;
            }

            _upgrade.interactable = !(_item.Item.PriceToUpgrade > _playerController.GetComponent<Gold>().GetGold);

            _priceToUpgrade.text = _item.Item.PriceToUpgrade.ToString();
            
            _itemImage.sprite = _item.Item.UIDisplay;
            _itemNameToUpgrade.text = _item.Item.name;
            
            _upgradedItem.Item = _itemUpgrader.GetCurrentItem().IsUpgradable() as ModifiableItem;
            _upgradedItemImage.sprite = _upgradedItem.Item.UIDisplay;
        }
    }
}