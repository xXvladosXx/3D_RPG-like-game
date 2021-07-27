using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeDialogueStrategy : InitializeDialogueStrategy
{
    [SerializeField] private GameObject _upgradeBar;
    [SerializeField] private Image _weaponImage;
    [SerializeField] private Button _upgrade;
    [SerializeField] private Button _next;
    [SerializeField] private Button _previous;
    [SerializeField] private TextMeshProUGUI _weaponName;
    
    private Inventory _playerInventory;
    private List<Item> _weaponItems;
    private PlayerController _player;
    
    public override void InitializeDialogMessage()
    {
        _weaponItems = new List<Item>();
        _player = FindObjectOfType<PlayerController>();

        _playerInventory = _player.GetComponent<PlayerInventory>().GetInventory;
        _upgradeBar.SetActive(true);
        
        foreach (var item in _playerInventory.GetInventory)
        {
            if (!item.IsStackable())
            {
                _weaponImage.sprite = item.GetItemSprite();
                _weaponName.text = item.GetItemSprite().name;
                _weaponItems.Add(item);
                print(item.GetItemLevel());
            }
        }
        
        if(_weaponItems.Count == 0) return;
        print(_weaponItems.Count);
        _next.onClick.AddListener(() => {ChangeWeapon("next");} );
        _previous.onClick.AddListener(() => {ChangeWeapon("prev");} );
        _upgrade.onClick.AddListener(() =>{UpgradeWeapon("upgrade");});

    }

    private void ChangeWeapon(string changeTo)
    {
        print(changeTo);
        print(_weaponImage.sprite);
    }

    private void UpgradeWeapon(string upgradeTo)
    {
        foreach (var item in _weaponItems)
        {
            if (!item.IsStackable())
            {
                if (_weaponImage.sprite == item.GetItemSprite())
                {
                    _playerInventory.RemoveItem(item);
                    print(item.GetUpgradedItemLevel(item.GetItemLevel()));
                    _weaponImage.sprite = item.GetUpgradedItemSprite();
                    _playerInventory.AddItem(item.GetUpgradedItem(item));
                }
            }
        }

    }
}
