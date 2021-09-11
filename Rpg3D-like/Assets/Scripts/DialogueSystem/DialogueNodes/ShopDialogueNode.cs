using DefaultNamespace;
using DialogueSystem.CoreDialogue;
using UI.PlayerBars.ShopBar;
using UnityEngine;

namespace DialogueSystem.DialogueNodes
{
    [CreateAssetMenu(menuName = "Speaker/Dialogue/Dialogue Node/Shop dialogue node")]
    public class ShopDialogueNode : DialogueNode
    {
        [SerializeField] private DialogueNode _nextDialogueNode;
        [SerializeField] private ShopUI _shopUI;
        public DialogueNode GetNextNode => _nextDialogueNode;
        
        public override bool InteractWithNode(DialogueNode dialogueNode)
        {
            GameObject shopUI = GameObject.FindWithTag("ShopBar");

            shopUI.GetComponent<IChangable>().Show();
            
            return _nextDialogueNode == dialogueNode;
        }

        public override void Accept(DialogueNodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}