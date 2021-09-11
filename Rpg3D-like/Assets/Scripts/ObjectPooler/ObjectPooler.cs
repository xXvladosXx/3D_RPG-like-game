using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObjectPooler
{
    public class ObjectPooler : MonoBehaviour
    {
        [SerializeField] public static ObjectPooler CurrentObjectPooler;
        [SerializeField] private GameObject _pooledObject;
        [SerializeField] private int _pooledAmount;
        [SerializeField] private bool _willGrow;

        private List<GameObject> _pooledObjects;

        private void Awake()
        {
            _pooledObjects = new List<GameObject>();
        
            CurrentObjectPooler = this;

            for (int i = 0; i < _pooledAmount; i++)
            {
                GameObject obj = Instantiate(_pooledObject, transform);
                obj.gameObject.SetActive(false);
                _pooledObjects.Add(obj);
            }
        }

        public GameObject GetPooledObject()
        {
            foreach (var pooledObject in _pooledObjects.Where(pooledObject => !pooledObject.gameObject.activeInHierarchy))
            {
                return pooledObject;
            }

            if (!_willGrow) return null;
        
            GameObject obj = Instantiate(_pooledObject);
            _pooledObjects.Add(obj);
            return obj;

        }
    }
}
