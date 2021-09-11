using DialogueSystem.DialogueNodes;

namespace DialogueSystem.CoreDialogue
{
    public interface DialogueNodeVisitor
    {
        void Visit(BasicDialogueNode basicDialogueNode);
        void Visit(DialogueChoiceNode dialogueChoiceNode);
        void Visit(UpgradeDialogueNode upgradeDialogueNode);
        void Visit(QuestDialogueNode questDialogueNode);
        void Visit(ShopDialogueNode questDialogueNode);

    }
}