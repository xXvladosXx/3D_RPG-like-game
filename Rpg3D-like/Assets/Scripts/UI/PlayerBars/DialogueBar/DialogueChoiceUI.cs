using System;
using DialogueSystem;
using DialogueSystem.CoreDialogue;
using DialogueSystem.DialogueNodes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class DialogueChoiceUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _choice;
        [SerializeField] private DialogueChannel _dialogueChannel;

        private DialogueNode _nextNode;

        public DialogueChoice Choice
        {
            set
            {
                _choice.text = value.Choice;
                _nextNode = value.DialogueNode;
            }
        }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _dialogueChannel.PopupRequestDialogueNode(_nextNode);
        }
    }
}