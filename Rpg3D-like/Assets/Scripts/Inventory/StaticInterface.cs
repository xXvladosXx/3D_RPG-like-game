using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class StaticInterface : UserInterface
    {
        [SerializeField] public GameObject[] _slots;
        
        public override void CreateSlots()
        {
            _slotOnUI = new Dictionary<GameObject, InventorySlot>();
            _index = 0;
        
            foreach (var inventorySlot in InventoryObject._inventory.Items)
            {
                var obj = _slots[_index];
            
                AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
                AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
                AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnBeginDrag(obj); });
                AddEvent(obj, EventTriggerType.EndDrag, delegate { OnEndDrag(obj); });
                AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

                _slotOnUI.Add(obj, inventorySlot);
                _index++;
            }
        }

    }
}
