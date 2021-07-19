using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using TMPro;
using UI.PlayerBars.BarsToInteract;
using UnityEngine;
using UnityEngine.UI;

public class DialogMessage : MonoBehaviour
{
    [SerializeField] private GameObject _dialogBar;
    [SerializeField] private Quest initializationQuest;
    [SerializeField] private QuestSystem _questSystem;
    
    private TextMeshProUGUI _textToSend;
    private GameObject _dialogTextNPC;
    private Button _acceptButton;
    private bool _hasButton = false;

    private void Awake()
    {
        _textToSend = GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.TryGetComponent(out PlayerController playerController))
        {
            
            _dialogBar.GetComponent<InteractableBar>().enabled = true;

            foreach (Transform child in _dialogBar.transform)
            {
                child.gameObject.SetActive(true);
            }
            
            _dialogBar.GetComponent<InteractableBar>().OnUIChanged += dialogUI =>
            {
                _dialogTextNPC = GameObject.FindGameObjectWithTag("DialogBar");
                _questSystem.SetQuest(initializationQuest);
            };
        }
    }

    public void QuestActivating()
    {
        
    }

    public void DeclineQuest()
    {
        _dialogBar.GetComponent<InteractableBar>().enabled = false;
        
        foreach (Transform child in _dialogBar.transform)
        {
            child.gameObject.SetActive(false);
        }
        
        _dialogBar.GetComponent<InteractableBar>().DeactivateBar();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            _dialogBar.GetComponent<InteractableBar>().enabled = false;
            
            foreach (Transform child in _dialogBar.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

}
