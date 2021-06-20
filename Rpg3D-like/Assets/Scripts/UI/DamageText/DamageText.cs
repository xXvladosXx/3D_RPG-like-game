using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private TextMeshPro _textMeshPro;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
    }

    public void SetDamageText(float amount)
    {
        transform.LookAt(Camera.main.transform);

        _textMeshPro.text = amount.ToString();
    }
}
