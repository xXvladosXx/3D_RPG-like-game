using DialogueSystem.DialogueNodes;
using UnityEngine;

namespace DialogueSystem.CoreDialogue
{
    [CreateAssetMenu(menuName = "Speaker/Dialogue/MainDialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private DialogueNode _firstDialogueNode;
        public DialogueNode GetDialogueNode => _firstDialogueNode;
    }
}