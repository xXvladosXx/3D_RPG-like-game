using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] public static ObjectPooler CurrentObjectPooler;
    [SerializeField] private ProjectileAttack _pooledObject;
    [SerializeField] private int _pooledAmount;
    [SerializeField] private bool _willGrow;

    private List<ProjectileAttack> _pooledObjects;

    private void Awake()
    {
        _pooledObjects = new List<ProjectileAttack>();
        
        CurrentObjectPooler = this;

        for (int i = 0; i < _pooledAmount; i++)
        {
            ProjectileAttack obj = Instantiate(_pooledObject);
            obj.gameObject.SetActive(false);
            _pooledObjects.Add(obj);
        }
    }

    public ProjectileAttack GetPooledObject()
    {
        foreach (var pooledObject in _pooledObjects)
        {
            if (!pooledObject.gameObject.activeInHierarchy)
                return pooledObject;
        }

        if (_willGrow)
        {
            ProjectileAttack obj = Instantiate(_pooledObject);
            _pooledObjects.Add(obj);
            return obj;
        }
        
        return null;
    }
}
