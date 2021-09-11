using System;
using System.Linq;
using Controller;
using UnityEngine;

namespace Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private InventoryObject _inventoryObject;
        [SerializeField] private InventoryObject _hotbarObject;
        public InventoryObject InventoryObject => _inventoryObject;
        private Collider _lastClickedObject;

        public event Action<ItemObject> OnItemPicked;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit raycastHit;
                Physics.Raycast(PlayerController.GetRay(), out raycastHit);
                _lastClickedObject = raycastHit.collider;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ItemTrigger>() != null && other == _lastClickedObject)
            {
                if (_hotbarObject._inventory.Items.Where(inventorySlot => inventorySlot != null)
                    .Any(inventorySlot => inventorySlot.itemData.Id == other.GetComponent<ItemTrigger>().GetItem.Data.Id && inventorySlot.ItemObject.Stackable))
                {
                    OnItemPicked?.Invoke(other.GetComponent<ItemTrigger>().GetItem);
                    _hotbarObject.AddItem(new ItemData(other.GetComponent<ItemTrigger>().GetItem), other.GetComponent<ItemTrigger>().GetItemAmount);
                    Destroy(other.gameObject);
                    return;
                }
            }
            
            if (other.GetComponent<ItemTrigger>() != null && _inventoryObject.HasEnoughPlace() && other == _lastClickedObject)
            {
                OnItemPicked?.Invoke(other.GetComponent<ItemTrigger>().GetItem);
                _inventoryObject.AddItem(new ItemData(other.GetComponent<ItemTrigger>().GetItem), other.GetComponent<ItemTrigger>().GetItemAmount);
                Destroy(other.gameObject);
            }
        }
    }
}
