using System;
using System.Collections;
using System.Collections.Generic;
using Inventories;
using UI.Cursor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Inventories
{
    public class ItemPickUp : MonoBehaviour, IRaycastable
    {
        [Serializable]
        public class ItemInfo
        {
            public Item Item;
            public int Amount;
            public GameObject ItemPrefab;
        }
    
        [SerializeField] private ItemInfo[] _itemsInfo;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController playerController))
            {
                foreach (var itemInfo in _itemsInfo)
                {
                    if(!other.GetComponent<PlayerInventory>().HasEnoughPlace()) return;
                        other.GetComponent<PlayerInventory>().InventoryPlacerItem(itemInfo.Item.ItemType, itemInfo.Amount);
                }
                
                Destroy(gameObject);
            }
        }

        public PlayerController.CursorType GetCursorType()
        {
            return PlayerController.CursorType.PickUp;
        }

        public bool HandleRaycast(PlayerController player)
        {
            if (Input.GetMouseButton(0))
            {
                player.GetComponent<Movement>().StartMoveToAction(gameObject.transform.position, 1f);
            }
            return true;
        }
    }
    
    public enum ItemType
    {
        Unexisted,
        
        Sword0,
        Sword1,
        Sword2,
        Sword3,
        Sword4,
    
        Bow0,
        Bow1,
        Bow2,
        Bow3,
        Bow4,
    
        HealthPotion,
    }
    
    public enum ItemCategory
    {
        None,
        Weapon,
        Armour,
        Potion

    }
}