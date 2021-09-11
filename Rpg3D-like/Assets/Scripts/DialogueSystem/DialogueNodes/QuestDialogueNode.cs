using DialogueSystem.CoreDialogue;
using Quests;
using UnityEngine;

namespace DialogueSystem.DialogueNodes
{
    [CreateAssetMenu(menuName = "Speaker/Dialogue/Dialogue Node/Quest dialogue node")]
    public class QuestDialogueNode : DialogueNode
    {
        [SerializeField] private DialogueNode _nextDialogueNode;
        [SerializeField] private Quest _quest;
        public DialogueNode GetNextNode => _nextDialogueNode;
        
        public override bool InteractWithNode(DialogueNode dialogueNode)
        {
            QuestSystem questSystem = FindObjectOfType<QuestSystem>();
            
            if(_quest == null) return _nextDialogueNode == dialogueNode;
             
            if (questSystem.GetQuest == null)
            {
                questSystem.SetQuest(_quest);
            }
            else
            {
                questSystem.AddQuest(_quest);
            }
                
            return _nextDialogueNode == dialogueNode;
        }

        public override void Accept(DialogueNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}