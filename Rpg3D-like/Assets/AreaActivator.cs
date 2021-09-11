using System;
using System.Collections;
using System.Collections.Generic;
using Controller;
using Quests;
using UnityEngine;

public class AreaActivator : MonoBehaviour
{
    private MeshCollider _collider;
    public event Action OnLocationEntered ;

    private void Awake()
    {
        _collider = GetComponent<MeshCollider>();
    }

    public void ActivateArea()
    {
        _collider.enabled = true;
    }

    public void DestroyArea()
    {
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController))
        {
            OnLocationEntered?.Invoke();
        }
    }
}
