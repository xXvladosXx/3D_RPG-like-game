using System;
using System.Collections;
using System.Collections.Generic;
using UI.PlayerBars.BarsToInteract;
using UnityEngine;

public class MainBar : MonoBehaviour, IPaused
{
    [SerializeField] private GameObject _none;
    [SerializeField] private GameObject _uiInventory;
    [SerializeField] private GameObject _statsBar;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            SwitchTo(_uiInventory);
            PauseGame(0);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchTo(_statsBar);
            PauseGame(0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchTo(_none);
            PauseGame(1);
        }
    }
    
    
    public void SwitchTo(GameObject switchTo)
    {
        if (switchTo.transform.parent != transform) return;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(child.gameObject == switchTo);
        }
    }

    public void PauseGame(float isPaused)
    {
        Time.timeScale = isPaused;
    }
}
