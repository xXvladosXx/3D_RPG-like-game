using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSwitcherUI : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;

    private void Start()
    {
        if(_mainMenu != null)
            SwitchTo(_mainMenu);
    }

    public void OnDisable()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
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
}
