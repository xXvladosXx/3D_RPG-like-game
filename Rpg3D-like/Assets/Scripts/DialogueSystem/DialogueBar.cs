using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.PlayerBars.BarsToInteract;
using UnityEngine;

public class DialogueBar : MonoBehaviour
{
    [SerializeField] private GameObject _uiToInteract;
    [SerializeField] private KeyCode _keyCode;
    [SerializeField] private TextMeshProUGUI _text;

    private bool _isBarActive = false;
    private bool _canInteract = false;

    public void SetCanInteract(bool talking)
    {
        _canInteract = talking;
    }
    public event Action<GameObject> OnDialogChanged; 
    private void Update()
    {
        if (!Input.GetKeyDown(_keyCode)) return;
        if(!_canInteract) return;
        
        if (_isBarActive)
        {                
            _isBarActive = false;
            DeactivateBar();
        }
        else
        {
            _isBarActive = true;
            ActivateBar();
        }
    }

    public void SetGreetingText(string npcGreetingText)
    {
        _text.text = npcGreetingText;
    }
    private void ActivateBar()
    {
        foreach (Transform child in _uiToInteract.transform)
        {
            child.gameObject.SetActive(_isBarActive);
        }

        OnDialogChanged?.Invoke(gameObject);
    }

    private void DeactivateBar()
    {
        foreach (Transform child in _uiToInteract.transform)
        {
            child.gameObject.SetActive(_isBarActive);
        }
        
        OnDialogChanged?.Invoke(gameObject);
    }

    public void ManualMaintainBar(bool activate)
    {
        _isBarActive = activate;
        foreach (Transform child in _uiToInteract.transform)
        {
            child.gameObject.SetActive(_isBarActive);
        }
    }

}