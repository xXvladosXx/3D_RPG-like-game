using System;
using Shops;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem.Interaction
{
    public class SpeakableObject : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnInteraction;

        private Customer _customer;
        private ShopSystem _shopSystem;

        private void Awake()
        {
            _customer = FindObjectOfType<Customer>();
            _shopSystem = GetComponent<ShopSystem>();
        }

        public void DoInteraction()
        {
            if (_shopSystem != null)
            {
                _customer.SetActiveShop(_shopSystem);
            }
                
            OnInteraction.Invoke();
        }
    }
}