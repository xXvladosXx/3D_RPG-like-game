using System;
using DialogueSystem;
using DialogueSystem.CoreDialogue;
using DialogueSystem.DialogueNodes;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace.UI
{
    public class TextDialogueControllerUI : MonoBehaviour, DialogueNodeVisitor
    {
        [SerializeField] private TextMeshProUGUI _speakerText;
        [SerializeField] private TextMeshProUGUI _dialogueText;
        [SerializeField] private RectTransform _choices;
        [SerializeField] private DialogueChoiceUI _dialogueChoiceUI;
        [SerializeField] private DialogueChannel _dialogueChannel;

        private bool _listenToInput = false;
        private DialogueNode _nextNode;

        private void Awake()
        {
            _dialogueChannel.OnDialogueNodeStart += OnDialogueNodeStart;
            _dialogueChannel.OnDialogueNodeEnd += OnDialogueNodeEnd;
            
            gameObject.SetActive(false);
            _choices.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _dialogueChannel.OnDialogueNodeEnd -= OnDialogueNodeEnd;
            _dialogueChannel.OnDialogueNodeStart -= OnDialogueNodeStart;
        }

        private void Update()
        {
            if (_listenToInput && Input.GetKeyDown(KeyCode.Return))
            {
                _dialogueChannel.PopupRequestDialogueNode(_nextNode);
            }
        }

        private void OnDialogueNodeEnd(DialogueNode dialogueNode)
        {
            _nextNode = null;
            _listenToInput = false;
            _dialogueText.text = "";
            _speakerText.text = "";

            foreach (Transform choice in _choices)
            {
                Destroy(choice.gameObject);
            }
            
            gameObject.SetActive(false);
            _choices.gameObject.SetActive(false);
        }

        private void OnDialogueNodeStart(DialogueNode dialogueNode)
        {
            gameObject.SetActive(true);

            _dialogueText.text = dialogueNode.GetSpeakerLine.GetText;
            _speakerText.text = dialogueNode.GetSpeakerLine.GetSpeaker.CharacterName;
            
            dialogueNode.Accept(this);
        }

        public void Visit(BasicDialogueNode basicDialogueNode)
        {
            _listenToInput = true;
            _nextNode = basicDialogueNode.GetNextNode;
        }

        public void Visit(DialogueChoiceNode dialogueChoiceNode)
        {
            _choices.gameObject.SetActive(true);

            foreach (DialogueChoice dialogueChoice in dialogueChoiceNode.GetChoices)
            {
                DialogueChoiceUI newChoice = Instantiate(_dialogueChoiceUI, _choices);
                newChoice.Choice = dialogueChoice;
            }
        }

        public void Visit(UpgradeDialogueNode upgradeDialogueNode)
        {
            _listenToInput = true;
            _nextNode = upgradeDialogueNode.GetNextNode;
        }

        public void Visit(QuestDialogueNode questDialogueNode)
        {
            _listenToInput = true;
            _nextNode = questDialogueNode.GetNextNode;
        }

        public void Visit(ShopDialogueNode questDialogueNode)
        {
            _listenToInput = true;
            _nextNode = questDialogueNode.GetNextNode;
        }
    }
}