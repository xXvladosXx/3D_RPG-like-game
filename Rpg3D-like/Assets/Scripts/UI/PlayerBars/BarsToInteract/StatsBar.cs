using System;
using System.Collections;
using System.Collections.Generic;
using UI.PlayerBars.BarsToInteract;
using UnityEngine;

public class StatsBar : MonoBehaviour
{
    private bool _isOpenedStats = true;

    private void OnEnable()
    {
        gameObject.SetActive(_isOpenedStats);
    }
   
}
