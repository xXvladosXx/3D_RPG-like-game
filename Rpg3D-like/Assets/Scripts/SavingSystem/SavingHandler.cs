using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavingHandler : MonoBehaviour
{
    private const string _defaultSaveFile = "QuickSave";

    public void StartNewGame(string saveFile)
    {
        StartCoroutine(LoadStartScene(saveFile));
    }
    
    public void ContinueGame(string saveFile = _defaultSaveFile)
    {
        saveFile = Path.GetFileNameWithoutExtension(saveFile);
        
        StartCoroutine(LoadScene(saveFile));
    }

    public void LoadGame(string saveFile = _defaultSaveFile)
    {
        StartCoroutine(LoadScene(saveFile));
    }
    
    private IEnumerator LoadScene(string saveFile)
    {
        DontDestroyOnLoad(gameObject);
        yield return GetComponent<SavingSystem>().LoadScene(saveFile);
    }
    
    private IEnumerator LoadStartScene(string saveFile)
    {
        DontDestroyOnLoad(gameObject);
        yield return SceneManager.LoadSceneAsync(2);
        Save(saveFile);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save(_defaultSaveFile);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Load(_defaultSaveFile);
        }
    }

    public void Load(string saveFile)
    {
        GetComponent<SavingSystem>().Load(saveFile);
    }

    public void Save(string saveFile)
    {
        GetComponent<SavingSystem>().Save(saveFile);
    }

    public IEnumerable<string> SaveList()
    {
        return GetComponent<SavingSystem>().SavesList();
    }

    
}
    
