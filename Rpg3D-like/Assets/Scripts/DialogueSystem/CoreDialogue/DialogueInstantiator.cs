using System;
using UnityEngine;

namespace DialogueSystem.CoreDialogue
{
    public class DialogueInstantiator : MonoBehaviour
    {
        [SerializeField] private DialogueChannel _dialogueChannel;

        private DialogueSequencer _dialogueSequencer;

        public event Action OnDialogueEnded; 

        private void Awake()
        {
            _dialogueSequencer = new DialogueSequencer();

            _dialogueSequencer.OnDialogueStart += OnDialogueStart;
            _dialogueSequencer.OnDialogueEnd += OnDialogueEnd;
            _dialogueSequencer.OnDialogueNodeStart += _dialogueChannel.PopupDialogueNodeStart;
            _dialogueSequencer.OnDialogueNodeEnd += _dialogueChannel.PopupDialogueNodeEnd;

            _dialogueChannel.OnDialogueRequest += _dialogueSequencer.StartDialogue;
            _dialogueChannel.OnDialogueNodeRequest += _dialogueSequencer.StartDialogueNode;
        }

        private void OnDestroy()
        {
            _dialogueChannel.OnDialogueNodeRequest -= _dialogueSequencer.StartDialogueNode;
            _dialogueChannel.OnDialogueRequest -= _dialogueSequencer.StartDialogue;

            _dialogueSequencer.OnDialogueEnd -= OnDialogueEnd;
            _dialogueSequencer.OnDialogueStart -= OnDialogueStart;
            _dialogueSequencer.OnDialogueNodeEnd -= _dialogueChannel.PopupDialogueNodeEnd;
            _dialogueSequencer.OnDialogueNodeStart -= _dialogueChannel.PopupDialogueNodeStart;

            _dialogueSequencer = null;
        }
        
        private void OnDialogueStart(Dialogue dialogue)
        {
            _dialogueChannel.PopupDialogueStart(dialogue);
        }

        private void OnDialogueEnd(Dialogue dialogue)
        {
            OnDialogueEnded?.Invoke();
            _dialogueChannel.PopupDialogueEnd(dialogue);
        }
    }
}