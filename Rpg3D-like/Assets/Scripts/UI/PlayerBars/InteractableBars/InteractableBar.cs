using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.PlayerBars.BarsToInteract;
using UnityEngine;

public class InteractableBar : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _uiToInteract;
    [SerializeField] private KeyCode _keyCode;

    private bool _isBarActive = false;
    public bool GetIsBarActive => _isBarActive;

    public void SetActive(bool isActive)
    {
        _isBarActive = isActive;
    }
    
    public event Action<GameObject> OnUIChanged; 
    private void Update()
    {
        if (Input.GetKeyDown(_keyCode))
        {
            if (_isBarActive)
            {                
                _isBarActive = false;
                OnUIChanged?.Invoke(gameObject);
            }
            else
            {
                _isBarActive = true;
                OnUIChanged?.Invoke(gameObject);
            }
            
        }
    }

    public void ActivateBar()
    {
        foreach (Transform child in _uiToInteract.transform)
        {
            child.gameObject.SetActive(_isBarActive);
        }
    }

    public void DeactivateBar()
    {
        foreach (Transform child in _uiToInteract.transform)
        {
            child.gameObject.SetActive(_isBarActive);
        }
    }
}
