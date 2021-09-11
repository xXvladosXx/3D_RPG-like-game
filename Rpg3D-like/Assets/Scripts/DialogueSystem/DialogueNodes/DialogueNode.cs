using DialogueSystem.CoreDialogue;
using DialogueSystem.Interaction;
using UnityEngine;

namespace DialogueSystem.DialogueNodes
{
    public abstract class DialogueNode : ScriptableObject
    {
        [SerializeField] private SpeakerLine _dialogueLine;
        public SpeakerLine GetSpeakerLine => _dialogueLine;

        public abstract bool InteractWithNode(DialogueNode dialogueNode);
        public abstract void Accept(DialogueNodeVisitor visitor);
    }
}