using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    public class DynamicInterface : UserInterface
    {
        [SerializeField] private GameObject _itemPrefab;

        public int xStart;
        public int yStart;
        public int columns;
        public int spaceY;
        public int spaceX;
        public override void CreateSlots()
        {
            _slotOnUI = new Dictionary<GameObject, InventorySlot>();

            foreach (var inventorySlot in InventoryObject._inventory.Items)
            {
                var obj = Instantiate(_itemPrefab,transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(_index);
            
                AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
                AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
                AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnBeginDrag(obj); });
                AddEvent(obj, EventTriggerType.EndDrag, delegate { OnEndDrag(obj); });
                AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            
                _slotOnUI.Add(obj, inventorySlot);
                _index++;
            }
        }
    
        private Vector2 GetPosition(int i)
        {
            return new Vector2(xStart + (spaceX * (i * 2 % columns)), yStart + (-spaceY * (i * 2 / columns)));
        }
    }
}
