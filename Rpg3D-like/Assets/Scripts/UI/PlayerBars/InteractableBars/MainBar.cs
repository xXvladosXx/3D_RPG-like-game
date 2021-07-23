using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI.PlayerBars.BarsToInteract;
using UnityEngine;

public class MainBar : MonoBehaviour
{
    private List<InteractableBar> _interactableBars;
    private PlayerController _player;
    private void Awake()
    {
        _interactableBars = new List<InteractableBar>();
        _player = FindObjectOfType<PlayerController>();
        
        foreach (InteractableBar interactableBar in FindObjectsOfType<InteractableBar>())
        {
            _interactableBars.Add(interactableBar);
        }
    }

    private void Start()
    {
        foreach (InteractableBar interactableBar in _interactableBars)
        {
            interactableBar.OnUIChanged += InteractableOnOnUIChanged;
        }
    }

    public void InteractableOnOnUIChanged(GameObject obj)
    {
        foreach (InteractableBar interactableBar in _interactableBars)
        {
            if (interactableBar.gameObject == obj)
            {
                interactableBar.ActivateBar();
            }
            
            if (interactableBar.gameObject != obj)
            {
                interactableBar.SetActive(false);
                interactableBar.DeactivateBar();
            }

        }

        bool atLeastOneActiveBar = _interactableBars.All(item => item.GetIsBarActive == false);
        PauseGame(Convert.ToSingle(atLeastOneActiveBar));
    }
    
    private void PauseGame(float isPaused)
    {
        _player.enabled = (Convert.ToBoolean(isPaused));
        Time.timeScale = isPaused;
    }
}