using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using TMPro;
using UI.PlayerBars.BarsToInteract;
using UnityEngine;
using UnityEngine.UI;

public class DialogueWithPlayer : MonoBehaviour
{
    [SerializeField] private DialogueBar _dialogueBar;
    [SerializeField] private DialogueInteraction _dialogueInteraction;
    [SerializeField] private string _greetingText;
    [SerializeField] private float _distanceToInteract = 3f;
    
    private GameObject _dialogueTextNPC;
    private Button _acceptButton;
    private bool _hasButton = false;

    private void Awake()
    {
        _dialogueBar = FindObjectOfType<DialogueBar>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            _dialogueInteraction = null;
            _dialogueBar.SetCanInteract(true);

            foreach (Transform child in _dialogueBar.transform)
            {
                child.gameObject.SetActive(true);
            }
            _dialogueBar.OnDialogChanged += dialogUI =>
            {
                _dialogueBar.SetGreetingText(_greetingText);
                
                if (_dialogueInteraction == null)
                {
                    _dialogueInteraction = FindObjectOfType<DialogueInteraction>();
                    _dialogueInteraction.SetDialogueInitializer(GetComponent<InitializeDialogueStrategy>());
                }

            };
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            _dialogueBar.ManualMaintainBar(false);
            
            foreach (Transform child in _dialogueBar.transform)
            {
                child.gameObject.SetActive(false);
            }
            _dialogueBar.SetCanInteract(false);

        }
        
    }

}
