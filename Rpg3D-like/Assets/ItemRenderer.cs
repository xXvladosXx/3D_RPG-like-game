using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ItemRenderer : MonoBehaviour
{
    public ModifiableItem Item;

    private void Awake()
    {
        AddEvent(EventTriggerType.PointerEnter, delegate { OnEnter(); });
        AddEvent(EventTriggerType.PointerExit, delegate { OnExit(); });
    }

    
    private void OnEnter()
    {
        if(Item!= null)
            Tooltip.EnableTooltip(Item.Description);
    }

    private void OnExit()
    {
        Tooltip.DisableTooltip();
    }
    
    private void AddEvent(EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry {eventID = type};
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }
}
