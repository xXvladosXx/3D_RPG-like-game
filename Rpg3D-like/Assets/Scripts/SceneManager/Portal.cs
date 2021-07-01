using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private int _sceneToLoad = -1;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private DestionationPortal _destionationPortal = DestionationPortal.A;

    enum DestionationPortal
    {
        A,
        B,
        C
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("PlayerCollided");

            StartCoroutine(WaitSceneToLoad()); 
        }
    }

    IEnumerator WaitSceneToLoad()
    {
        DontDestroyOnLoad(gameObject);
        SavingHandler savingHandler = FindObjectOfType<SavingHandler>();
        savingHandler.Save();
        
        yield return SceneManager.LoadSceneAsync(_sceneToLoad);;

        savingHandler.Load();
        Portal otherPortal = GetOtherPortal();
        UpdatePlayer(otherPortal);
        
        Destroy(gameObject);
    }

    private void UpdatePlayer(Portal otherPortal)
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<NavMeshAgent>().enabled = false;

        player.transform.position = otherPortal._spawnPoint.position;
        player.transform.rotation = otherPortal._spawnPoint.rotation;
        
        player.GetComponent<NavMeshAgent>().enabled = true;

    }

    private Portal GetOtherPortal()
    {
        foreach (Portal portal in FindObjectsOfType<Portal>())
        {
            if (portal == this) continue;
            
            if(portal._destionationPortal == _destionationPortal)
                return portal;
        }

        return null;
    }
}
