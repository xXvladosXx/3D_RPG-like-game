using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingHandler : MonoBehaviour
{
    private const string _defaultSaveFile = "save";

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Load();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }

    public void Load()
    {
        GetComponent<SavingSystem>().Load(_defaultSaveFile);
    }

    public void Save()
    {
        GetComponent<SavingSystem>().Save(_defaultSaveFile);
    }
}
    
