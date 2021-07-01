using UnityEngine;

public class LoadedObjects : MonoBehaviour
{
    [SerializeField] private GameObject persistentObjectPrefab;
 
    private static bool _hasSpawned = false;
    private void Awake()
    {
        if (_hasSpawned)
        {
            return;
        }
 
        SpawnPersistentObjects();
        _hasSpawned = true;
    }
 
    private void SpawnPersistentObjects()
    {
        var persistentObject = Instantiate(persistentObjectPrefab);
        DontDestroyOnLoad(persistentObject);
    }
}
