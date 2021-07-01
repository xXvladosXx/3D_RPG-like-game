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
        Dictionary<string, object> capturedStates = LoadFile(saveFile);
        CaptureState(capturedStates);
        SaveFile(saveFile, capturedStates);
    }

    private void CaptureState(Dictionary<string, object> capturedStates)
    {
        foreach (SaveableEntity saveableEntity in FindObjectsOfType<SaveableEntity>())
        {
            capturedStates[saveableEntity.GetUniqueIdentifier()] = saveableEntity.CaptureState();
        }
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
        if (!File.Exists(path))
        {
            return new Dictionary<string, object>();
        }
        
        using (FileStream fileStream = File.Open(path, FileMode.Open))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(fileStream);
        }     
    }
    private void RestoreState(Dictionary<string, object> state)
    {
        foreach (SaveableEntity saveableEntity in FindObjectsOfType<SaveableEntity>())
        {
            string uniqueID = saveableEntity.GetUniqueIdentifier();

            if (state.ContainsKey(uniqueID))
            {
                saveableEntity.RestoreState(state[uniqueID]);
            }
        }
    }
    
    private string GetPathFromSaveFile(string saveFile)
    {
        return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
    }
}
