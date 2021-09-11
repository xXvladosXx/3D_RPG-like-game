using System;
using System.Linq;
using DialogueSystem.CoreDialogue;
using UnityEngine;

namespace DialogueSystem.DialogueNodes
{
    [Serializable]
    public class DialogueChoice
    {
        public string Choice;
        public DialogueNode DialogueNode;
    }
    
    [CreateAssetMenu(menuName = "Speaker/Dialogue/Node/Choice")]
    public class DialogueChoiceNode : DialogueNode
    {
        [SerializeField] private DialogueChoice[] _choices;
        public DialogueChoice[] GetChoices => _choices;
        
        public override bool InteractWithNode(DialogueNode dialogueNode)
        {
            return _choices.Any(x => x.DialogueNode == dialogueNode);
        }

        public override void Accept(DialogueNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}