using Controller;
using Inventory;
using UnityEngine;

namespace UI
{
    public class HotBarInteraction : MonoBehaviour
    {
        [SerializeField] private KeyCode _firstSlot;
        [SerializeField] private KeyCode _secondSlot;
        [SerializeField] private KeyCode _thirdSlot;
        [SerializeField] private KeyCode _fourthSlot;

        private StaticInterface _staticInterface;

        private void Awake()
        {
            _staticInterface = GetComponent<StaticInterface>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(_firstSlot))
            {
                UseItemOnIndex( _staticInterface.InventoryObject._inventory.Items[0]);
            }
        
            if (Input.GetKeyDown(_secondSlot))
            {
                UseItemOnIndex( _staticInterface.InventoryObject._inventory.Items[1]);
            }
        
            if (Input.GetKeyDown(_thirdSlot))
            {
                UseItemOnIndex( _staticInterface.InventoryObject._inventory.Items[2]);
            }
        
            if (Input.GetKeyDown(_fourthSlot))
            {
                UseItemOnIndex( _staticInterface.InventoryObject._inventory.Items[3]);
            }
        }

        private void UseItemOnIndex(InventorySlot inventorySlot)
        {
            if(inventorySlot == null) return;
            if (inventorySlot.Amount <= 1)
            {
                inventorySlot.itemData.Id = -1;
                return;
            }
        
            inventorySlot.ItemObject.EquipItem(FindObjectOfType<PlayerController>());
            inventorySlot.Amount--;
        }
    }
}