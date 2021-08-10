using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueInteraction : MonoBehaviour
{
    [SerializeField] private InitializeDialogueStrategy _npcDialogueStrategyInitialize;
    [SerializeField] private Button _button;
    [SerializeField] private List<Transform> _children;
    private void Awake()
    {
        _button = GetComponent<Button>();
        foreach (Transform child in transform)
        {
            _children.Add(child);
        }
    }
    public void SetDialogueInitializer(InitializeDialogueStrategy npc)
    {
        _npcDialogueStrategyInitialize = npc;
        _button.onClick.AddListener(InitializeDialogMessage);
        
        _npcDialogueStrategyInitialize.OnDialogChanged += () =>
        {
            _button.interactable = true;
        };
    }

    private void InitializeDialogMessage()
    {
        _button.interactable = false;
        _npcDialogueStrategyInitialize.InitializeDialogMessage();
    }

    private void OnEnable()
    {
        _button.interactable = true;
        _button.enabled = true;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        _button.interactable = false;
        _button.enabled = false;
    }
}
