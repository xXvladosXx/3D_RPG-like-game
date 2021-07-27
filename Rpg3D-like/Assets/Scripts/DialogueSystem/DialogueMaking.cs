using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using TMPro;
using UI.PlayerBars.BarsToInteract;
using UnityEngine;
using UnityEngine.UI;

public class DialogueMaking : MonoBehaviour
{
    [SerializeField] private DialogueBar _dialogueBar;
    [SerializeField] private DialogueInteraction _dialogueInteraction;
    
    private TextMeshProUGUI _textToSend;
    private GameObject _dialogueTextNPC;
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
            _dialogueBar.GetComponent<InteractableBar>().enabled = true;
            _dialogueInteraction = null;

            foreach (Transform child in _dialogueBar.transform)
            {
                child.gameObject.SetActive(true);
            }

            _dialogueBar.GetComponent<InteractableBar>().OnUIChanged += dialogUI =>
            {
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
            _dialogueBar.GetComponent<InteractableBar>().enabled = false;
            
            foreach (Transform child in _dialogueBar.transform)
            {
                child.gameObject.SetActive(false);
            }

            GetComponent<InitializeDialogueStrategy>().enabled =false;
        }
    }

}
