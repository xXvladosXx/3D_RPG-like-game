using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextToInherit : MonoBehaviour
{
    private TextMeshProUGUI _dialogText;

    private void Awake()
    {
        _dialogText = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(string text)
    {
        _dialogText.text = text;
    }
}
