using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingHandler : MonoBehaviour
{
    private const string _defaultSaveFile = "save";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GetComponent<SavingSystem>().Save(_defaultSaveFile);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            GetComponent<SavingSystem>().Load(_defaultSaveFile);
        }
    }
}
    
