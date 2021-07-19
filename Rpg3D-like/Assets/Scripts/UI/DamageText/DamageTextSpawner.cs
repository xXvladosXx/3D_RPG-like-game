using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField] private DamageText _damageText;
    private TextMeshPro _textMeshPro;
  
    public void SpawnText(float damage)
    {
                
        foreach (Transform child in transform)
        {
            _textMeshPro = child.gameObject.GetComponent<TextMeshPro>();
            _textMeshPro.text = damage.ToString();
        }
        
        DamageText damageInstance = Instantiate(_damageText, transform);
        damageInstance.SetDamageText(damage);

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject, 0.3f);
        }
    }

    private void Update()
    {
        if(Camera.main != null)
            transform.LookAt(Camera.main.transform);
    }
}
