using System;
using System.Collections.Generic;
using Controller;
using Controller.States;
using DefaultNamespace.Scenes;
using DialogueSystem.CoreDialogue;
using UnityEngine;

namespace DialogueSystem.Interaction
{
    public class SpeakableInstantiator : MonoBehaviour, IUnchangeableState
    {
        [SerializeField] private Transform _tooltipToTalk;
        [SerializeField] private ChangeUI[] _UIs;
        
        private DialogueInstantiator _dialogueInstantiator;
        private List<SpeakableObject> _nearbyInteractable = new List<SpeakableObject>();
        private ActionScheduler _actionScheduler;

        private void Awake()
        {
            _dialogueInstantiator = GetComponent<DialogueInstantiator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            
            _dialogueInstantiator.OnDialogueEnded += _actionScheduler.Cancel;
        }

        private bool HasNearbyInteractables()
        {
            return _nearbyInteractable.Count != 0;
        }

        private void Update()
        {
            if (_nearbyInteractable.Count > 0)
            {
                _tooltipToTalk.gameObject.SetActive(true);
            }
            else
            {
                _tooltipToTalk.gameObject.SetActive(false);
            }
            
            if (HasNearbyInteractables() && Input.GetKeyDown(KeyCode.A))
            {
                _nearbyInteractable[0].DoInteraction();
                _actionScheduler.StartAction(this);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            SpeakableObject speakableObject = other.GetComponent<SpeakableObject>();

            if (speakableObject != null)
            {
                _tooltipToTalk.gameObject.SetActive(true);
                _nearbyInteractable.Add(speakableObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            SpeakableObject speakableObject = other.GetComponent<SpeakableObject>();

            if (speakableObject != null)
            {
                _nearbyInteractable.Remove(speakableObject);
                foreach (var ui in _UIs)
                {
                    ui.Hide();
                }
            }
        }

        public void Cancel()
        {
            
        }
    }
}