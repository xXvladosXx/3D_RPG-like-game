using System;
using SavingSystem;
using UnityEngine;

namespace Inventory
{
    public class Gold : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _amount;

        public event Action OnGoldChanged;
        public float GetGold => _amount;

        public void UpdateGold(float amount)
        {
            _amount += amount;
            OnGoldChanged?.Invoke();
        }

        public object CaptureState()
        {
            return _amount;
        }

        public void RestoreState(object state)
        {
            _amount = (float) state;
        }
    }
}

