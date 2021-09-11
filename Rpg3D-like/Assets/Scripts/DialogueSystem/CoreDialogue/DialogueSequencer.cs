using DialogueSystem.DialogueNodes;
using UnityEngine;

namespace DialogueSystem.CoreDialogue
{
    public class DialogueSequencer
    {
        public delegate void DialogueCallback(Dialogue dialogue);

        public delegate void DialogueNodeCallback(DialogueNode dialogueNode);

        public DialogueCallback OnDialogueStart;
        public DialogueCallback OnDialogueEnd;

        public DialogueNodeCallback OnDialogueNodeStart;
        public DialogueNodeCallback OnDialogueNodeEnd;

        private Dialogue _currentDialogue;
        private DialogueNode _currentNode;

        public void StartDialogue(Dialogue dialogue)
        {
            if(_currentDialogue != null) return;

            _currentDialogue = dialogue;
            OnDialogueStart?.Invoke(_currentDialogue);
            StartDialogueNode(dialogue.GetDialogueNode);
        }

        public void StartDialogueNode(DialogueNode dialogueNode)
        {
            if (_currentNode != null)
            {
                _currentNode.InteractWithNode(dialogueNode);
            }
                
            StopDialogueNode(_currentNode);
            _currentNode = dialogueNode;

            if (_currentNode != null)
            {
                OnDialogueNodeStart?.Invoke(_currentNode);
            }
            else
            {
                EndDialogue(_currentDialogue);
            }
        }

        private void EndDialogue(Dialogue dialogue)
        {
            if (_currentDialogue != dialogue) return;
            
            StopDialogueNode(_currentNode);
            OnDialogueEnd?.Invoke(_currentDialogue);
            _currentDialogue = null;
        }

        private void StopDialogueNode(DialogueNode dialogueNode)
        {
            if (_currentNode != dialogueNode) return;
            
            OnDialogueNodeEnd?.Invoke(_currentNode);
            _currentNode = null;
        }

        private bool CanStartNode(DialogueNode dialogueNode)
        {
            return (_currentNode != null && dialogueNode != null);
        }
    }
}