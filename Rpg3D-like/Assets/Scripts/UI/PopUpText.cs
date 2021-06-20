using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
    private TextMeshProUGUI _textToSpawn;

    private void Awake()
    {
        _textToSpawn = GetComponent<TextMeshProUGUI>();
    }

    public void PopMessage(string text)
    {
        _textToSpawn.gameObject.SetActive(true);
        print("textong");
        _textToSpawn.text = text;
    }
}
