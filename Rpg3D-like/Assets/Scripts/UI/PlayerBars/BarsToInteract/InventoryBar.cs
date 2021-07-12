using System;
using System.Collections;
using System.Collections.Generic;
using UI.PlayerBars.BarsToInteract;
using UnityEngine;

public class InventoryBar : MonoBehaviour
{
    [SerializeField] private UIInventory _uiInventory;
    private bool _isOpenedInventory = true;
    private void OnEnable()
    {
        _uiInventory.InteractWithUI(_uiInventory.transform, _isOpenedInventory);
    }
}
