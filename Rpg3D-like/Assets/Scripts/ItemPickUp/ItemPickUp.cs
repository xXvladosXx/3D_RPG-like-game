using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour, IClickable
{
    [SerializeField] private Item.ItemType _itemType;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            other.GetComponent<PlayerController>().InventoryPlacer(this);
            Destroy(gameObject);
        }
    }
    
    public Item.ItemType GetItemType()
    {
        return _itemType;
    }

    public void OnHoverEnter()
    {
        
    }

    public void OnHoverExit()
    {
        throw new NotImplementedException();
    }
}
