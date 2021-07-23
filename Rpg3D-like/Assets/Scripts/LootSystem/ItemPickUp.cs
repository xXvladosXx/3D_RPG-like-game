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
        public Item.ItemType _itemType;
        public int _amount;
        public int _minSoul;
        public int _maxSoul;
    }

    [SerializeField] private ItemInfo[] _myClass;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            foreach (var itemInfo in _myClass)
            {
                itemInfo._amount = Random.Range(itemInfo._minSoul, itemInfo._maxSoul);
                other.GetComponent<PlayerController>().InventoryPlacerItem(itemInfo._itemType, itemInfo._amount);
            }
            
            Destroy(gameObject);
        }
    }
}
