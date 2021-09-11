using DialogueSystem.CoreDialogue;
using UnityEngine;

namespace DialogueSystem.DialogueNodes
{
    [CreateAssetMenu(menuName = "Speaker/Dialogue/Dialogue Node/Basic dialogue node")]
    public class BasicDialogueNode : DialogueNode
    {
        [SerializeField] private DialogueNode _nextDialogueNode;
        public DialogueNode GetNextNode => _nextDialogueNode;
        
        public override bool InteractWithNode(DialogueNode dialogueNode)
        {
            return _nextDialogueNode == dialogueNode;
        }

        public override void Accept(DialogueNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}