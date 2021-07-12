using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SaveLoadUI : MonoBehaviour
{
    [SerializeField] private Transform _content;
    [SerializeField] private GameObject _buttonPref;

    private SavingHandler _savingHandler;
    private void OnEnable()
    {
        _savingHandler = FindObjectOfType<SavingHandler>();
        
        if(_savingHandler == null) return;
        
        foreach (Transform child in _content)
        {
            Destroy(child.gameObject);
        }
        foreach (var save in _savingHandler.SaveList())
        {
            GameObject button = Instantiate(_buttonPref, _content);
            button.GetComponentInChildren<TextMeshProUGUI>().text = save;
            
            Button button1 = button.GetComponent<Button>();
            button1.onClick.AddListener((() =>
            {
                _savingHandler.LoadGame(save);
            }));
        }
    }

}
