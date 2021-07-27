using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemPickUp : MonoBehaviour
{
    [Serializable]
    public class ItemInfo
    {
        public Item.ItemType ItemType;
        public GameObject Item;
        public int Amount;
    }

    [SerializeField] private ItemInfo[] _itemsInfo;
    [SerializeField] private int _minSoul;
    [SerializeField] private int _maxSoul;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            foreach (var itemInfo in _itemsInfo)
            {
                if(itemInfo.Amount == 0)
                    itemInfo.Amount = Random.Range(_minSoul, _maxSoul);
                
                if(!other.GetComponent<PlayerInventory>().HasEnoughPlace()) return;
                    other.GetComponent<PlayerInventory>().InventoryPlacerItem(itemInfo.ItemType, itemInfo.Amount);
            }
            
            Destroy(gameObject);
        }
    }
}
