using DialogueSystem.DialogueNodes;
using UnityEngine;

namespace DialogueSystem.CoreDialogue
{
    [CreateAssetMenu(menuName = "Speaker/Dialogue/Dialogue Channel")]
    public class DialogueChannel : ScriptableObject
    {
        public delegate void DialogueCallback(Dialogue dialogue);

        public DialogueCallback OnDialogueRequest;
        public DialogueCallback OnDialogueStart;
        public DialogueCallback OnDialogueEnd;

        public delegate void DialogueNodeCallback(DialogueNode dialogueNode);

        public DialogueNodeCallback OnDialogueNodeRequest;
        public DialogueNodeCallback OnDialogueNodeStart;
        public DialogueNodeCallback OnDialogueNodeEnd;

        public void PopupRequestDialogue(Dialogue dialogue)
        {
            OnDialogueRequest?.Invoke(dialogue);
        }

        public void PopupDialogueStart(Dialogue dialogue)
        {
            OnDialogueStart?.Invoke(dialogue);
        }

        public void PopupDialogueEnd(Dialogue dialogue)
        {
            OnDialogueEnd?.Invoke(dialogue);
        }

        public void PopupRequestDialogueNode(DialogueNode dialogueNode)
        {
            OnDialogueNodeRequest?.Invoke(dialogueNode);
        }

        public void PopupDialogueNodeStart(DialogueNode dialogueNode)
        {
            OnDialogueNodeStart?.Invoke(dialogueNode);
        }

        public void PopupDialogueNodeEnd(DialogueNode dialogueNode)
        {
            OnDialogueNodeEnd?.Invoke(dialogueNode);
        }
    }
}