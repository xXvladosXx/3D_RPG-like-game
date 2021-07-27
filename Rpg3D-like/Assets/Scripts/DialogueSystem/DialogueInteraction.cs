using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueInteraction : MonoBehaviour
{
    [SerializeField] private InitializeDialogueStrategy _npcDialogueStrategyInitialize;
    [SerializeField] private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    public void SetDialogueInitializer(InitializeDialogueStrategy npc)
    {
        if(npc != _npcDialogueStrategyInitialize && _npcDialogueStrategyInitialize !=null)
            _button.onClick.RemoveListener(_npcDialogueStrategyInitialize.InitializeDialogMessage);

        _npcDialogueStrategyInitialize = npc;
        _button.onClick.AddListener(_npcDialogueStrategyInitialize.InitializeDialogMessage);
    }

    private void OnEnable()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
       
    }
}
