using System;
using System.Linq;
using Inventories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UpgradeSystem;

namespace UI.PlayerBars.UpgradeBar
{
    public class UpgradeUI : MonoBehaviour
    {
        [SerializeField] private GameObject _weaponPopUpUpgrade;
        [SerializeField] private UpgradeDialogueStrategy _upgradeDialogueStrategy;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _itemResources;
        [SerializeField] private Image _itemImage;
        [SerializeField] private Button _upgrade;
        [SerializeField] private Button _next;
        [SerializeField] private Button _previous;

        private int _inventorySize;
        private int _currentIndex;
        private Item _itemToUpgrade;

        public event Action<Item> OnWeaponUpgraded;
        private void Awake()
        {
            _previous.onClick.AddListener(() =>
            {
                _currentIndex -= 1;
                SetItem(_upgradeDialogueStrategy.GetItems[_currentIndex]);
            });
            _next.onClick.AddListener(() =>
            {
                _currentIndex += 1;
                SetItem(_upgradeDialogueStrategy.GetItems[_currentIndex]);
            });

            _upgrade.onClick.AddListener(InitUpgrade);
           
        }

        private void Update()
        {
            if (_currentIndex < 0)
            {
                _next.interactable = false;
                _previous.interactable = false;
                _upgrade.interactable = false;
                
                return;
            }
            
            _upgrade.interactable = true;
            _next.interactable = CanMoveRight();
            _previous.interactable = CanMoveLeft();
        }

        private bool CanMoveLeft()
        {
            return _currentIndex < _inventorySize && _currentIndex != 0;
        }

        private bool CanMoveRight()
        {
            return _currentIndex >= 0 && _currentIndex != _inventorySize-1;
        }

        private void InitUpgrade()
        {
            if(!_upgradeDialogueStrategy.HasEnoughMoney(_itemToUpgrade)){ return;}
            
            OnWeaponUpgraded?.Invoke(_upgradeDialogueStrategy.GetItems[_currentIndex]);
            GameObject weaponUpgrade = Instantiate(_weaponPopUpUpgrade, transform.parent);
            
            string itemName = _itemToUpgrade.GetUpgradedItem().IItem.ToString();
            itemName = itemName.Split('.').Last();
            
            weaponUpgrade.GetComponent<UIWeaponUpgraded>().SetWeaponSprite(_itemToUpgrade.GetUpgradedItemSprite());
            weaponUpgrade.GetComponent<UIWeaponUpgraded>().SetWeaponLevelText(itemName);
            
            Destroy(weaponUpgrade, 1.2f);
        }

        public void SetUpgradeDialog(UpgradeDialogueStrategy dialogueStrategy)
        {
            _upgradeDialogueStrategy = dialogueStrategy;
            
            _inventorySize = _upgradeDialogueStrategy.GetItems.Count;

            _currentIndex = _inventorySize-1;

            SetItem(_upgradeDialogueStrategy.GetItems.LastOrDefault());
        }

        public void SetItem(Item item)
        {
            if(item == null) return;
            
            _itemToUpgrade = item;
            _itemImage.sprite = _itemToUpgrade.GetItemSprite();
            
            string itemName = _itemToUpgrade.IItem.ToString();
            _itemName.text = itemName.Split('.').Last();

            _itemResources.text = _itemToUpgrade.GetUpgradePrice().ToString();
        }
    }
}