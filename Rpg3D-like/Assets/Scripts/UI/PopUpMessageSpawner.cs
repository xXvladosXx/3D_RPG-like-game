using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class PopUpMessageSpawner : MonoBehaviour
{
    [SerializeField] private PopUpText _popUpText;
    [SerializeField] TextMeshProUGUI _textMeshPro;

    public void SpawnText(string text)
    {
        TextMeshProUGUI popUpText = Instantiate(_textMeshPro);
        
        Destroy(popUpText,.3f);
    }
}
