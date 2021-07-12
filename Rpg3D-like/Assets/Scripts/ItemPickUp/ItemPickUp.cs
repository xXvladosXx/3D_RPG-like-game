using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private Item.ItemType _itemType;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            other.GetComponent<PlayerController>().InventoryPlacerItem(this);
            Destroy(gameObject);
        }
    }
    
    public Item.ItemType GetItemType()
    {
        return _itemType;
    }
}
