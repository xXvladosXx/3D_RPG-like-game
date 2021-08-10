using System;
using Shops;
using UI.Cursor;
using UnityEngine;

namespace DialogueSystem
{
    [RequireComponent(typeof(ShopSystem))]
    public class TransactionDialogueStrategy : InitializeDialogueStrategy, ITransactable
    {
        [SerializeField] private GameObject _shopUI;
        private ShopSystem _shop;
        private Customer _customer;
        private void Awake()
        {
            _customer = FindObjectOfType<Customer>();
            _shop = GetComponent<ShopSystem>();
        }

        public override event Action OnDialogChanged;

        public override void InitializeDialogMessage()
        {
            Transact();            
        }

        public void Transact()
        {
            _shopUI.SetActive(true);
            _customer.SetActiveShop(_shop);
        }

      
    }
}