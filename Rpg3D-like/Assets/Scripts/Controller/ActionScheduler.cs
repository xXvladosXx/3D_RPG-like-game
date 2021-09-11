using System;
using UnityEngine;

namespace Controller
{
    public class ActionScheduler : MonoBehaviour, IAction
    {
        private IAction _currentAction;

        public IAction GetCurrentAction => _currentAction;
        
        public void StartAction(IAction action)
        {
            if(_currentAction == action) return;

            if (_currentAction != null)
            {
                _currentAction.Cancel();
            }
        
            _currentAction = action;
        }
    
        public void Cancel()
        {
            StartAction(null);
        }

    }
}
