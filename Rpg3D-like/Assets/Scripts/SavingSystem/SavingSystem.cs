 using System;
 using System.Collections;
using System.Collections.Generic;
 using System.IO;
 using System.Runtime.Serialization.Formatters.Binary;
 using System.Text;
 using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    public void Save(string saveFile)
    {
        SaveFile(saveFile, CaptureState());
    }

    private Dictionary<string, object> CaptureState()
    {
        Dictionary<string, object> capturedStates = new Dictionary<string, object>();
        foreach (SaveableEntity saveableEntity in FindObjectsOfType<SaveableEntity>())
        {
            Debug.Log(saveableEntity.gameObject);
            capturedStates[saveableEntity.GetUniqueIdentifier()] = saveableEntity.CaptureState();
        }
        
        return capturedStates;
    }
    private void SaveFile(string saveFile, object captureState)
    {
        string path = GetPathFromSaveFile(saveFile);
        print("Saving tp " + path);

        using (FileStream fileStream = File.Open(path, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(fileStream, captureState);
        }
    }

    public void Load(string saveFile)
    {
        RestoreState(LoadFile(saveFile));
    }

    private Dictionary<string, object> LoadFile(string saveFile)
    {
        string path = GetPathFromSaveFile(saveFile);
        using (FileStream fileStream = File.Open(path, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(fileStream);
        }     
    }
    private void RestoreState(object state)
    {
        Dictionary<string, object> stateToRestore = (Dictionary<string, object>)state;
        foreach (SaveableEntity saveableEntity in FindObjectsOfType<SaveableEntity>())
        {
            saveableEntity.RestoreState(stateToRestore[saveableEntity.GetUniqueIdentifier()]);
        }
    }
    
    private string GetPathFromSaveFile(string saveFile)
    {
        return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }
}
