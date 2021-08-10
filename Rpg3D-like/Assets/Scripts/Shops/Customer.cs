using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shops
{
    public class Customer : MonoBehaviour
    {
        private ShopSystem _activeShop = null;
        public ShopSystem GetCurrentShop => _activeShop;
        
        public event Action OnActiveShopChange;
        
        public void SetActiveShop(ShopSystem activeShop)
        {
            if(_activeShop!=null)
                _activeShop.SetCustomer(null);
            
            _activeShop = activeShop;
            
            if(_activeShop!=null)
                _activeShop.SetCustomer(this);
            
            if (_activeShop != null)
                OnActiveShopChange?.Invoke();
        }
    }
}