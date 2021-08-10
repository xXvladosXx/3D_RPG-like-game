using System.Collections;
using System.Collections.Generic;
using Inventories;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventories
{
    public class ItemSlot : MonoBehaviour, IDropHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                eventData.pointerDrag.GetComponent<Drag>().SetInSlot(true);
            
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition =
                    GetComponent<RectTransform>().anchoredPosition;
            }
        }
    }

}
