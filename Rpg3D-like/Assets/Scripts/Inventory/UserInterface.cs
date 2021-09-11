using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Controller.States;
using DefaultNamespace.Scenes;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Inventory
{
    public abstract class UserInterface : MonoBehaviour, IUnchangeableState
    {
        [SerializeField] public InventoryObject InventoryObject;

        private ActionScheduler _actionScheduler;
        
        protected int _index = 0;
        protected Dictionary<GameObject, InventorySlot> _slotOnUI = new Dictionary<GameObject, InventorySlot>();

        private void Awake()
        {
            _actionScheduler = GetComponentInParent<ActionScheduler>();
        }

        private void Start()
        {
            foreach (var inventorySlot in InventoryObject._inventory.Items)
            {
                inventorySlot.Parent = this;
            }

            CreateSlots();
            AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
            AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
        }

        private void OnExitInterface(GameObject o)
        {
            MouseData.UI = null;
        }

        private void OnEnterInterface(GameObject o)
        {
            MouseData.UI = o.GetComponent<UserInterface>();
        }

        public abstract void CreateSlots();

        protected void OnEnter(GameObject o)
        {
            MouseData.ItemHoverOver = o;
            if (_slotOnUI[o].ItemObject != null)
            {
                Tooltip.EnableTooltip(_slotOnUI[o].ItemObject.Description);
            }
        }

        protected void OnExit(GameObject o)
        {
            Tooltip.DisableTooltip();
            MouseData.ItemHoverOver = null;
        }

        protected void OnBeginDrag(GameObject o)
        {
            var mouseObj = new GameObject();
            var rt = mouseObj.AddComponent<RectTransform>();
            var cu = mouseObj.AddComponent<ChangeUI>();
            rt.sizeDelta = new Vector2(50, 50);
            
            mouseObj.transform.SetParent(transform.parent.parent.parent);
            mouseObj.GetComponent<Canvas>().enabled = true;
            mouseObj.GetComponent<Canvas>().overrideSorting = true;
            
            cu.Show();
            _actionScheduler.StartAction(this);
            
            if (_slotOnUI[o].itemData.Id >= 0)
            {
                var img = mouseObj.AddComponent<Image>();
                img.sprite =_slotOnUI[o].ItemObject.UIDisplay;
                img.raycastTarget = false;
            }

            MouseData.TempItemDragged = mouseObj;
            MouseData.TempItemDragged.GetComponent<RectTransform>().SetAsLastSibling(); 
        }


        protected void OnDrag(GameObject o)
        {
            if (MouseData.TempItemDragged == null) return;

            MouseData.TempItemDragged.GetComponent<RectTransform>().position = Input.mousePosition;
            MouseData.TempItemDragged.GetComponent<RectTransform>().SetAsLastSibling();
        }

        protected void OnEndDrag(GameObject o)
        {
            Destroy(MouseData.TempItemDragged);
            _actionScheduler.Cancel();
            
            if (MouseData.UI == null)
            {
                // Debug.Log(_slotOnUI[o].Item.Prefab);
                // GameObject itemPrefab = Instantiate(_slotOnUI[o].Item.Prefab,_playerInventory.transform.position, Quaternion.identity);
                // Debug.Log(itemPrefab);
            
                _slotOnUI[o].RemoveItem();
                return;
            }
        
            if (MouseData.ItemHoverOver && MouseData.UI != this)
            {
                InventorySlot mouseHoverSlot = MouseData.UI._slotOnUI[MouseData.ItemHoverOver];
                InventoryObject.SwapItem(_slotOnUI[o], mouseHoverSlot);
                if (mouseHoverSlot.itemData.Id > 0)
                {
                    mouseHoverSlot.ItemObject.EquipItem(GetComponentInParent<PlayerController>());
                }
            
                return;
            }
        
            if (MouseData.ItemHoverOver)
            {
                InventorySlot mouseHoverSlot = MouseData.UI._slotOnUI[MouseData.ItemHoverOver];
                InventoryObject.SwapItem(_slotOnUI[o], mouseHoverSlot);
            }
        }

        private void Update()
        {
            UpdateSlots();
        }

        public void UpdateSlots()
        {
            foreach (var slot in _slotOnUI)
            {
                if (slot.Value.itemData.Id >= 0)
                {
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite =
                        slot.Value.ItemObject.UIDisplay;
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                    slot.Key.GetComponentInChildren<TextMeshProUGUI>().text =
                        slot.Value.Amount == 1 ? "" : slot.Value.Amount.ToString();
                }
                else
                {
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                    slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                    slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
                }
            }
        }

        protected void AddEvent(GameObject gameObject, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
            var eventTrigger = new EventTrigger.Entry {eventID = type};
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }

        public void Cancel()
        {
            
        }
    }

    public static class MouseData
    {
        public static UserInterface UI;
        public static GameObject TempItemDragged;
        public static GameObject ItemHoverOver;
    }
}