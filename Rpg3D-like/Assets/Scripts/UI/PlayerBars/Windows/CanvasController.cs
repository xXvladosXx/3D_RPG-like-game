using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DefaultNamespace.Scenes;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    private List<GameObject> _uiToDisable = new List<GameObject>();
    public List<GameObject> GetList => _uiToDisable;

    public bool CanShowNewUIs { get; set; } = true;

    private int _currentOrder = 0;
    public void AddUI(GameObject gameObject)
    {
        _uiToDisable.Add(gameObject);   
        
        _currentOrder++;
        gameObject.GetComponent<Canvas>().sortingOrder = _currentOrder;
    }

    public void RemoveUI(GameObject gameObject)
    {
        _uiToDisable.Remove(gameObject);
        
        gameObject.GetComponent<Canvas>().sortingOrder = 0;
    }
}
