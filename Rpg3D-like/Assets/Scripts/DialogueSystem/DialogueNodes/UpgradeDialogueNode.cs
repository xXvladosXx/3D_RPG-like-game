using DefaultNamespace;
using DialogueSystem.CoreDialogue;
using UI.PlayerBars.UpgradeBar;
using UnityEngine;

namespace DialogueSystem.DialogueNodes
{
    [CreateAssetMenu(menuName = "Speaker/Dialogue/Dialogue Node/Upgrade dialogue node")]
    public class UpgradeDialogueNode : DialogueNode
    {
        [SerializeField] private UpgradeUI _upgradeUI;
        [SerializeField] protected DialogueNode _nextDialogueNode;
        public DialogueNode GetNextNode => _nextDialogueNode;
        
        public override bool InteractWithNode(DialogueNode dialogueNode)
        {
            GameObject upgradeUI = GameObject.FindWithTag("UpgradeBar");

            upgradeUI.GetComponent<IChangable>().Show();

            return _nextDialogueNode == dialogueNode;
        }

        public override void Accept(DialogueNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}